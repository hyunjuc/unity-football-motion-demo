using UnityEngine;

public class DirectionArrows : MonoBehaviour
{
    public float arrowLength = 2f;
    public float lineWidth = 0.12f;
    public float heightOffset = 0.02f; // 발밑에 딱 붙게, 그리드랑 안 겹치게 살짝만 띄움

    private LineRenderer moveArrow;
    private LineRenderer faceArrow;
    private Vector3 lastPosition;
    private Vector3 moveDelta;

    void Start()
    {
        lastPosition = transform.position;
        moveArrow = CreateArrow(Color.red);
        faceArrow = CreateArrow(Color.blue);
    }

    LineRenderer CreateArrow(Color color)
    {
        GameObject go = new GameObject("Arrow_" + color);
        LineRenderer lr = go.AddComponent<LineRenderer>();

        Shader shader = Shader.Find("Sprites/Default");
        Material mat = new Material(shader);
        mat.renderQueue = 4000; // Overlay 큐로 맨 나중에 그리기
        lr.material = mat;

        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.positionCount = 5;
        lr.useWorldSpace = true;
        return lr;
    }

    void Update()
    {
        moveDelta = transform.position - lastPosition;
        lastPosition = transform.position;

        // 캐릭터 다리 사이(루트 포지션) 기준
        Vector3 basePos = transform.position + Vector3.up * heightOffset;

        Vector3 moveDir = moveDelta.sqrMagnitude > 0.0001f ? moveDelta.normalized : transform.forward;
        DrawArrow(moveArrow, basePos, moveDir);
        DrawArrow(faceArrow, basePos, transform.forward);
    }

    void DrawArrow(LineRenderer lr, Vector3 origin, Vector3 dir)
    {
        Vector3 tip = origin + dir * arrowLength;
        Vector3 back = -dir * 0.3f;
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized * 0.25f;

        lr.SetPosition(0, origin);
        lr.SetPosition(1, tip);
        lr.SetPosition(2, tip + back + right);
        lr.SetPosition(3, tip);
        lr.SetPosition(4, tip + back - right);
    }
}