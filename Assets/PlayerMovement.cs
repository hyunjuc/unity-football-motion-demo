using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float sprintMultiplier = 2f;
    public float rotSpeed = 20f;

    [Header("Kick / Ball")]
    public Ball ball;
    public Transform ballAnchor; // 플레이어 소유 드리블 앵커
    public float kickForce = 8f;

    [Header("Tackle (NPC 공 뺏기)")]
    public NPCFollower npc;
    public Transform npcBallAnchor;
    public float stealDistance = 1.2f;
    public float tackleContactTime = 0.46f;
    public float tackleClipLength = 1.767f;
    public float tackleCooldown = 2f;

    [Header("Tackle 슬라이딩")]
    public float tackleSlideSpeed = 5f;
    public float tackleSlideDuration = 1.1f;
    public float tackleSlideAngleOffset = 0f;

    [Header("Tackled 리액션 길이")]
    public float tackledInPlaceDuration = 2.3f;
    public float tackledInRunDuration = 2.733f;

    private CharacterController controller;
    private Animator animator;
    private bool isKicking = false;
    private bool isTackled = false;
    private bool isTackling = false;
    private float tackleCooldownTimer = 0f;
    private float tackleSlideTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (tackleCooldownTimer > 0f)
            tackleCooldownTimer -= Time.deltaTime;

        if (!isKicking && !isTackled && !isTackling && Input.GetKeyDown(KeyCode.Space))
        {
            if (ball != null && ball.IsOwnedBy(ballAnchor))
            {
                animator.SetTrigger("Kick");
                isKicking = true;
                Invoke(nameof(KickBall), 0.4f);
                Invoke(nameof(EndKick), 1.3f);
            }
        }

        if (!isKicking && !isTackled && !isTackling && Input.GetKeyDown(KeyCode.F))
        {
            TryTackleNPC();
        }

        if (isTackling)
        {
            if (tackleSlideTimer < tackleSlideDuration)
            {
                Vector3 slideDir = Quaternion.Euler(0f, tackleSlideAngleOffset, 0f) * transform.forward;
                controller.Move(slideDir * tackleSlideSpeed * Time.deltaTime);
                tackleSlideTimer += Time.deltaTime;
            }
            return;
        }

        if (isKicking || isTackled) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v);

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = sprint ? speed * sprintMultiplier : speed;

        if (move.magnitude > 0.1f)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);

            float animSpeed = move.magnitude * (sprint ? sprintMultiplier : 1f);
            animator.SetFloat("Speed", animSpeed);

            float angleDiff = Vector3.SignedAngle(transform.forward, move, Vector3.up);
            animator.SetFloat("TurnDirection", Mathf.Clamp(angleDiff / 90f, -1f, 1f));

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(move), rotSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    void LateUpdate()
    {
        if (isTackled) return;

        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }

    void KickBall()
    {
        if (ball != null)
            ball.Kick(transform.forward, kickForce);
    }

    void EndKick()
    {
        isKicking = false;
    }

    void TryTackleNPC()
    {
        if (npc == null || ball == null || npcBallAnchor == null || ballAnchor == null) return;
        if (tackleCooldownTimer > 0f) return;
        if (ball.IsOwnedBy(ballAnchor)) return; // 이미 내가 공을 갖고 있으면 태클 자체를 시작 안 함

        isTackling = true;
        tackleSlideTimer = 0f;
        animator.SetTrigger("Tackle");
        Invoke(nameof(DoStealFromNPC), tackleContactTime);
        Invoke(nameof(EndPlayerTackle), tackleClipLength);
    }

    void DoStealFromNPC()
    {
        if (ball == null || npc == null || npcBallAnchor == null) return;
        if (!ball.IsOwnedBy(npcBallAnchor)) return; // 그 사이 NPC가 공을 잃었으면 실패

        float dist = Vector3.Distance(transform.position, npc.transform.position);
        if (dist > stealDistance) return;

        ball.SetOwner(ballAnchor);
        npc.GetTackled();
    }

    void EndPlayerTackle()
    {
        isTackling = false;
        tackleCooldownTimer = tackleCooldown;
    }

    public void GetTackled()
    {
        if (isTackled || isKicking) return;

        isTackled = true;
        bool moving = animator.GetFloat("Speed") > 0.15f;

        if (moving)
        {
            animator.SetTrigger("TackledInRun");
            Invoke(nameof(EndTackle), tackledInRunDuration);
        }
        else
        {
            animator.SetTrigger("TackledInPlace");
            Invoke(nameof(EndTackle), tackledInPlaceDuration);
        }
    }

    void EndTackle()
    {
        isTackled = false;
    }
}