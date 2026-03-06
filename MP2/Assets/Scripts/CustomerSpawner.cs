using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Spawn")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform exitPointA;
    public Transform exitPointB;
    public float exitRadius = 2f;
    public float spawnInterval = 0.6f;
    public int maxAlive = 20;

    [Header("Spawn Spacing")]
    public float spawnClearRadius = 0.6f;
    public LayerMask customerLayerMask;

    [Header("Stations")]
    public StationGenerator[] stations;

    int aliveCount;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TrySpawnOne()
    {
        if (bank == null || customerPrefab == null || spawnPoint == null)
            return;

        if (exitPointA == null && exitPointB == null)
            return;

        if (stations == null || stations.Length == 0)
            return;

        if (aliveCount >= maxAlive)
            return;

        if (SpawnAreaBlocked())
            return;

        StationGenerator chosen = PickStationWithStock();
        if (chosen == null)
            return;

        Transform chosenExitTransform = PickExitPoint();
        if (chosenExitTransform == null)
            return;

        Vector3 chosenExitPosition = GetRandomExitPosition(chosenExitTransform);

        GameObject go = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        var customer = go.GetComponent<CustomerWalkerBuyer>();
        if (customer == null)
        {
            Debug.LogWarning("Customer prefab missing CustomerWalkerBuyer component.");
            Destroy(go);
            return;
        }

        aliveCount++;
        customer.Init(bank, chosen, chosenExitPosition, OnCustomerDespawned);
    }

    Transform PickExitPoint()
    {
        if (exitPointA != null && exitPointB != null)
            return Random.value < 0.5f ? exitPointA : exitPointB;

        if (exitPointA != null)
            return exitPointA;

        return exitPointB;
    }

    Vector3 GetRandomExitPosition(Transform exitTransform)
    {
        Vector2 offset2D = Random.insideUnitCircle * exitRadius;
        return exitTransform.position + new Vector3(offset2D.x, 0f, offset2D.y);
    }

    bool SpawnAreaBlocked()
    {
        Collider[] hits = Physics.OverlapSphere(
            spawnPoint.position,
            spawnClearRadius,
            customerLayerMask
        );

        return hits.Length > 0;
    }

    StationGenerator PickStationWithStock()
    {
        List<StationGenerator> valid = new List<StationGenerator>();

        for (int i = 0; i < stations.Length; i++)
        {
            StationGenerator s = stations[i];
            if (s == null) continue;
            if (!s.gameObject.activeInHierarchy) continue;

            if (HasAtLeastOne(bank, s.produces))
                valid.Add(s);
        }

        if (valid.Count == 0)
            return null;

        return valid[Random.Range(0, valid.Count)];
    }

    static bool HasAtLeastOne(ResourceBank bank, StationGenerator.FoodType type)
    {
        switch (type)
        {
            case StationGenerator.FoodType.HotDog:   return bank.hotDogs >= 1f;
            case StationGenerator.FoodType.Fries:    return bank.fries >= 1f;
            case StationGenerator.FoodType.Sandwich: return bank.sandwiches >= 1f;
            case StationGenerator.FoodType.Lasagna:  return bank.lasagne >= 1f;
        }

        return false;
    }

    void OnCustomerDespawned()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPoint.position, spawnClearRadius);
        }

        if (exitPointA != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(exitPointA.position, exitRadius);
        }

        if (exitPointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(exitPointB.position, exitRadius);
        }
    }
}