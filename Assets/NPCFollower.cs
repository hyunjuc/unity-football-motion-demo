using UnityEngine;
using UnityEngine.AI;

public class NPCFollower : MonoBehaviour
{
    public Transform player;
    public Ball ball;
    public Transform npcBallAnchor;

    [Header("Tackle / Steal")]
    public float stealDistance = 1.2f;
    public float tackleCooldown = 2f;
    public float tackleClipLength = 1.767f;
    public float tackleContactTime = 0.46f;

    [Header("공 소유 후 행동")]
    public float fleeDistance = 5f;
    public float fleeTriggerDistance = 5f;

    [Header("태클당함 리액션")]
    public float tackledInPlaceDuration = 2.3f;
    public float tackledInRunDuration = 2.733f;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerMovement playerMovement;
    private bool isTackling = false;
    private bool isTackled = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // 캐시된 변수 대신 매 프레임 Ball한테 직접 소유권 확인 (동기화 어긋남 방지)
        bool hasBall = ball != null && ball.IsOwnedBy(npcBallAnchor);

        if (isTackled)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (hasBall)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            if (distToPlayer < fleeTriggerDistance)
            {
                Vector3 fleeDir = (transform.position - player.position).normalized;
                agent.SetDestination(transform.position + fleeDir * fleeDistance);
            }
            else
            {
                agent.ResetPath();
            }
        }
        else if (player != null)
        {
            agent.SetDestination(player.position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        float distToPlayerForSteal = Vector3.Distance(transform.position, player.position);
        if (!hasBall && !isTackling && cooldownTimer <= 0f && distToPlayerForSteal < stealDistance && ball != null)
        {
            StartTackle();
        }
    }

    void StartTackle()
    {
        isTackling = true;
        animator.SetTrigger("Tackle");
        Invoke(nameof(DoSteal), tackleContactTime);
        Invoke(nameof(EndTackle), tackleClipLength);
    }

    void DoSteal()
    {
        // 접촉 시점에 아직 상대(플레이어)가 갖고 있을 때만 성공 처리
        if (ball != null && npcBallAnchor != null && !ball.IsOwnedBy(npcBallAnchor))
        {
            ball.SetOwner(npcBallAnchor);
        }

        if (playerMovement != null)
            playerMovement.GetTackled();
    }

    void EndTackle()
    {
        isTackling = false;
        cooldownTimer = tackleCooldown;
    }

    public void GetTackled()
    {
        if (isTackled) return; // isTackling 가드는 제거 — 공격 애니메이션 중이어도 리액션이 우선

        isTackled = true;

        bool moving = agent.velocity.magnitude > 0.15f;
        if (moving)
        {
            animator.SetTrigger("TackledInRun");
            Invoke(nameof(EndTackled), tackledInRunDuration);
        }
        else
        {
            animator.SetTrigger("TackledInPlace");
            Invoke(nameof(EndTackled), tackledInPlaceDuration);
        }

        agent.ResetPath();
    }

    void EndTackled()
    {
        isTackled = false;
    }

    void LateUpdate()
    {
        if (isTackling || isTackled) return;

        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }
}