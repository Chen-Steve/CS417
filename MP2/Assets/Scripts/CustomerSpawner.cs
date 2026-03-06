using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Spawn")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public float spawnInterval = 4f;
    public int maxAlive = 6;

    [Header("Stations")]
    public StationGenerator[] stations;

    float timer;
    int aliveCount;

    void Update()
    {
        if (bank == null || customerPrefab == null || spawnPoint == null || exitPoint == null)
            return;

        if (stations == null || stations.Length == 0)
            return;

        if (aliveCount >= maxAlive)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        StationGenerator chosen = PickStationWithStock();
        if (chosen == null)
            return;

        GameObject go = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        var customer = go.GetComponent<CustomerWalkerBuyer>();
        if (customer == null)
        {
            Debug.LogWarning("Customer prefab missing CustomerWalkerBuyer component.");
            Destroy(go);
            return;
        }

        aliveCount++;
        customer.Init(bank, chosen, exitPoint, OnCustomerDespawned);
    }

    StationGenerator PickStationWithStock()
    {

        var valid = new System.Collections.Generic.List<StationGenerator>();

        for (int i = 0; i < stations.Length; i++)
        {
            var s = stations[i];
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
}