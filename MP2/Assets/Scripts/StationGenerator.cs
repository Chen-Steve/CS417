using UnityEngine;

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

    int currentVisualCount = 0;
    float lastFoodCount;

    bool isActive = false;

    void OnEnable()
    {
        StartGenerating();
    }

    void StartGenerating()
    {
        if (bank == null || isActive)
            return;

        switch (produces)
        {
            case FoodType.HotDog:
                bank.hotDogRate += foodPerSecond;
                break;

            case FoodType.Fries:
                bank.friesRate += foodPerSecond;
                break;

            case FoodType.Sandwich:
                bank.sandwichRate += foodPerSecond;
                break;

            case FoodType.Lasagna:
                bank.lasagnaRate += foodPerSecond;
                break;
        }

        isActive = true;
    }

    void OnDisable()
    {
        if (bank == null || !isActive)
            return;

        switch (produces)
        {
            case FoodType.HotDog:
                bank.hotDogRate -= foodPerSecond;
                break;

            case FoodType.Fries:
                bank.friesRate -= foodPerSecond;
                break;

            case FoodType.Sandwich:
                bank.sandwichRate -= foodPerSecond;
                break;

            case FoodType.Lasagna:
                bank.lasagnaRate -= foodPerSecond;
                break;
        }
    }

    void Update()
    {
        if (bank == null) return;

        float currentCount = GetCurrentFoodAmount();

        if (Mathf.FloorToInt(currentCount) > Mathf.FloorToInt(lastFoodCount))
        {
            SpawnFood();
        }

        lastFoodCount = currentCount;
    }

    float GetCurrentFoodAmount()
    {
        switch (produces)
        {
            case FoodType.HotDog: return bank.hotDogs;
            case FoodType.Fries: return bank.fries;
            case FoodType.Sandwich: return bank.sandwiches;
            case FoodType.Lasagna: return bank.lasagne;
        }

        return 0f;
    }

    void SpawnFood()
    {
        if (foodPrefab == null || spawnPoint == null)
            return;

        if (currentVisualCount >= maxVisualFood)
            return;

        GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);

        currentVisualCount++;

        // when destroyed, reduce counter
        Destroy(food, 15f);
        StartCoroutine(DecreaseCountAfterDelay(15f));
    }

    // coroutine
    System.Collections.IEnumerator DecreaseCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentVisualCount--;
    }
}