using UnityEngine;

public class AnimDebugHUD : MonoBehaviour
{
    public Animator animator;
    private CharacterController controller;
    private Vector3 lastPosition;
    private Vector3 moveDelta;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // 매 프레임 실제 이동한 방향(루트모션 대신 우리가 직접 이동시키는 벡터) 계산
        moveDelta = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    void OnGUI()
    {
        if (animator == null) return;

        GUIStyle style = new GUIStyle { fontSize = 16, normal = { textColor = Color.yellow } };

        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        string clipNames = "";
        foreach (var c in clipInfo)
            clipNames += $"{c.clip.name} ({c.weight:F2})  ";

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        GUI.Label(new Rect(10, 10, 600, 30), $"State: {stateInfo.fullPathHash}  Clips: {clipNames}", style);
        GUI.Label(new Rect(10, 35, 600, 30), $"Speed: {animator.GetFloat("Speed"):F2}  Direction: {animator.GetFloat("Direction"):F2}  TurnDir: {animator.GetFloat("TurnDirection"):F2}", style);
        GUI.Label(new Rect(10, 60, 600, 30), $"이동 방향(월드): {moveDelta.normalized}", style);
    }

    void OnDrawGizmos()
    {
        // Scene 뷰에서 이동 방향 화살표(빨강) + 캐릭터 정면(파랑) 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, moveDelta.normalized * 1.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * 1.5f);
    }
}