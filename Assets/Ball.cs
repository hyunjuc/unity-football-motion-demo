using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform dribbleAnchor;
    public float respawnDelay = 1.5f;
    public float radius = 0.11f;
    public float groundY = 0f;

    private Rigidbody rb;
    private bool isKicked = false;
    private float kickTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (dribbleAnchor != null)
        {
            Collider playerCol = dribbleAnchor.root.GetComponent<CharacterController>();
            Collider ballCol = GetComponent<Collider>();
            if (playerCol != null && ballCol != null)
                Physics.IgnoreCollision(ballCol, playerCol);
        }
    }

    void Update()
    {
        if (!isKicked)
        {
            Vector3 newPos = dribbleAnchor.position;
            newPos.y = groundY + radius;

            Vector3 delta = newPos - transform.position;
            delta.y = 0f;

            transform.position = newPos;

            if (delta.sqrMagnitude > 0.000001f)
            {
                Vector3 axis = Vector3.Cross(Vector3.up, delta).normalized;
                float distance = delta.magnitude;
                float angle = (distance / radius) * Mathf.Rad2Deg;
                transform.Rotate(axis, angle, Space.World);
            }
        }
        else
        {
            kickTimer += Time.deltaTime;
            if (kickTimer >= respawnDelay)
                Respawn();
        }
    }

    public void Kick(Vector3 direction, float force)
    {
        isKicked = true;
        kickTimer = 0f;
        rb.isKinematic = false;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public bool IsOwnedBy(Transform anchor)
    {
        return dribbleAnchor == anchor;
    }

    public void SetOwner(Transform newAnchor)
    {
        dribbleAnchor = newAnchor;
        isKicked = false;

        rb.isKinematic = false; // velocity 초기화하려면 잠깐 풀어줘야 경고 안 뜸
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    void Respawn()
    {
        isKicked = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Vector3 pos = dribbleAnchor.position;
        pos.y = groundY + radius;
        transform.position = pos;
    }
}