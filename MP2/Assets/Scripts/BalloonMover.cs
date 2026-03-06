using UnityEngine;

public class BalloonMover : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 2f;
    public float maxDistance = 40f;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}