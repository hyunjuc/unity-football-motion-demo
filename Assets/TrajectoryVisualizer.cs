using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    [Header("궤적 설정")]
    public float pastDuration = 1f;
    public float futureDuration = 1f;
    public float sampleInterval = 0.1f; // 점 간격 (언리얼 느낌 내려면 0.08~0.15 추천)
    public float heightOffset = 0.05f;
    public float dotSize = 0.12f;
    public Color pastColor = new Color(1f, 0.8f, 0.1f);
    public Color futureColor = new Color(0.2f, 0.9f, 1f);

    private List<Vector3> pastPositions = new List<Vector3>();
    private float sampleTimer = 0f;

    private Vector3 lastFramePos;
    private Vector3 lastForward;
    private Vector3 currentVelocity;
    private float currentAngularVelocity; // 도(degree)/초

    private List<Transform> pastDots = new List<Transform>();
    private List<Transform> futureDots = new List<Transform>();

    int MaxPastSamples => Mathf.Max(2, Mathf.RoundToInt(pastDuration / sampleInterval));
    int FutureSamples => Mathf.Max(2, Mathf.RoundToInt(futureDuration / sampleInterval));

    void Start()
    {
        lastFramePos = transform.position;
        lastForward = transform.forward;

        for (int i = 0; i < MaxPastSamples; i++)
        {
            pastPositions.Add(transform.position);
            pastDots.Add(CreateDot(pastColor));
        }
        for (int i = 0; i < FutureSamples; i++)
            futureDots.Add(CreateDot(futureColor));
    }

    Transform CreateDot(Color color)
    {
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(dot.GetComponent<Collider>());
        dot.transform.localScale = Vector3.one * dotSize;

        var rend = dot.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = color;
        mat.renderQueue = 4000; // 항상 위에 보이게
        rend.material = mat;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.receiveShadows = false;

        return dot.transform;
    }

    void Update()
    {
        // 현재 이동 속도(벡터) 추정
        Vector3 frameDelta = transform.position - lastFramePos;
        if (Time.deltaTime > 0f)
            currentVelocity = Vector3.Lerp(currentVelocity, frameDelta / Time.deltaTime, 10f * Time.deltaTime);
        lastFramePos = transform.position;

        // 현재 회전 각속도(도/초) 추정 — TurnDirection 파라미터 대신 실제 회전량으로 계산
        float angleDelta = Vector3.SignedAngle(lastForward, transform.forward, Vector3.up);
        float rawAngularVel = Time.deltaTime > 0f ? angleDelta / Time.deltaTime : 0f;
        currentAngularVelocity = Mathf.Lerp(currentAngularVelocity, rawAngularVel, 10f * Time.deltaTime);
        lastForward = transform.forward;

        // 과거 위치 샘플링
        sampleTimer += Time.deltaTime;
        if (sampleTimer >= sampleInterval)
        {
            sampleTimer = 0f;
            pastPositions.Add(transform.position);
            if (pastPositions.Count > MaxPastSamples)
                pastPositions.RemoveAt(0);
        }

        UpdatePastDots();
        UpdateFutureDots();
    }

    void UpdatePastDots()
    {
        for (int i = 0; i < pastDots.Count; i++)
            pastDots[i].position = pastPositions[i] + Vector3.up * heightOffset;
    }

    void UpdateFutureDots()
    {
        // 현재 속도 + 각속도를 이용해 원호를 따라가듯 스텝별로 시뮬레이션 (회전 중이면 자연스럽게 휘어짐)
        Vector3 simPos = transform.position;
        Vector3 simVel = currentVelocity;

        for (int i = 0; i < futureDots.Count; i++)
        {
            simVel = Quaternion.Euler(0f, currentAngularVelocity * sampleInterval, 0f) * simVel;
            simPos += simVel * sampleInterval;
            futureDots[i].position = simPos + Vector3.up * heightOffset;
        }
    }
}