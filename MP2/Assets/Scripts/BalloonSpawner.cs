using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform spawnPoint;
    public Vector3 moveDirection = Vector3.right;
    public float travelDistance = 40f;

    [Header("Random Spawn Settings")]
    public float minSpawnDelay = 10f;
    public float maxSpawnDelay = 25f;

    bool balloonActive = false;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            if (!balloonActive)
            {
                SpawnBalloon();
            }
        }
    }

    void SpawnBalloon()
    {
        GameObject balloon = Instantiate(balloonPrefab, spawnPoint.position, Quaternion.identity);

        BalloonMover mover = balloon.GetComponent<BalloonMover>();
        mover.direction = moveDirection.normalized;
        mover.maxDistance = travelDistance;

        balloonActive = true;

        balloon.GetComponent<DestroyNotifier>().OnDestroyed += () =>
        {
            balloonActive = false;
        };
    }
}