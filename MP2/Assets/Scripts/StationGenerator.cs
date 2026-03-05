using UnityEngine;
using System.Collections.Generic;

public class StationGenerator : MonoBehaviour
{
    public ResourceBank bank;

    public enum FoodType
    {
        HotDog,
        Fries,
        Sandwich,
        Lasagna
    }

    [Header("Visual Spawn Settings")]
    public FoodType produces;
    public float foodPerSecond = 1f;
    public Transform spawnPoint;
    public int maxVisualFood = 3;
    public GameObject foodPrefab;
    public float visualLifetime = 15f;
    public bool replaceOldestVisualWhenFull = true;

    readonly List<GameObject> activeVisuals = new List<GameObject>();
    float visualSpawnProgress;
    float lastTrackedFoodAmount;
    bool hasTrackedFoodAmount;

    bool isActive = false;

    void OnEnable()
    {
        StartGenerating();

        if (bank != null)
        {
            lastTrackedFoodAmount = GetCurrentFoodAmount();
            hasTrackedFoodAmount = true;
        }
    }

    void StartGenerating()
    {
        if (bank == null || isActive)
            return;

        ApplyRateDelta(foodPerSecond);

        isActive = true;
    }

    void OnDisable()
    {
        if (bank == null || !isActive)
            return;

        ApplyRateDelta(-foodPerSecond);
        isActive = false;
        hasTrackedFoodAmount = false;
    }

    public void IncreaseProductionRate(float amount)
    {
        if (amount <= 0f)
            return;

        SetProductionRate(foodPerSecond + amount);
    }

    public void SetProductionRate(float newRate)
    {
        float clampedRate = Mathf.Max(0f, newRate);

        if (Mathf.Approximately(clampedRate, foodPerSecond))
            return;

        if (bank != null && isActive)
        {
            float delta = clampedRate - foodPerSecond;
            ApplyRateDelta(delta);
        }

        foodPerSecond = clampedRate;
    }

    public void MultiplyProductionRate(float multiplier)
    {
        if (multiplier <= 0f)
            return;

        SetProductionRate(foodPerSecond * multiplier);
    }

    void ApplyRateDelta(float delta)
    {
        switch (produces)
        {
            case FoodType.HotDog:
                bank.hotDogRate += delta;
                break;

            case FoodType.Fries:
                bank.friesRate += delta;
                break;

            case FoodType.Sandwich:
                bank.sandwichRate += delta;
                break;

            case FoodType.Lasagna:
                bank.lasagnaRate += delta;
                break;
        }
    }

    void Update()
    {
        if (bank == null || !isActive)
            return;

        float currentFoodAmount = GetCurrentFoodAmount();

        if (!hasTrackedFoodAmount)
        {
            lastTrackedFoodAmount = currentFoodAmount;
            hasTrackedFoodAmount = true;
        }

        float producedDelta = currentFoodAmount - lastTrackedFoodAmount;
        lastTrackedFoodAmount = currentFoodAmount;

        if (producedDelta <= 0f)
            return;

        visualSpawnProgress += producedDelta;

        while (visualSpawnProgress >= 1f)
        {
            if (!SpawnFood())
            {
                break;
            }

            visualSpawnProgress -= 1f;
        }
    }

    float GetCurrentFoodAmount()
    {
        switch (produces)
        {
            case FoodType.HotDog:
                return bank.hotDogs;
            case FoodType.Fries:
                return bank.fries;
            case FoodType.Sandwich:
                return bank.sandwiches;
            case FoodType.Lasagna:
                return bank.lasagne;
            default:
                return 0f;
        }
    }

    bool SpawnFood()
    {
        if (foodPrefab == null || spawnPoint == null)
            return false;

        activeVisuals.RemoveAll(visual => visual == null);

        if (maxVisualFood > 0 && activeVisuals.Count >= maxVisualFood)
        {
            if (!replaceOldestVisualWhenFull)
                return false;

            GameObject oldest = activeVisuals[0];
            activeVisuals.RemoveAt(0);

            if (oldest != null)
                Destroy(oldest);
        }

        GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);
        activeVisuals.Add(food);

        // when destroyed, reduce counter
        StartCoroutine(DestroyVisualAfterDelay(food, visualLifetime));

        return true;
    }

    // coroutine
    System.Collections.IEnumerator DestroyVisualAfterDelay(GameObject food, float delay)
    {
        yield return new WaitForSeconds(delay);

        activeVisuals.Remove(food);

        if (food != null)
            Destroy(food);
    }
}