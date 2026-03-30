using UnityEngine;

public class ResourceBank : MonoBehaviour
{    
    [Header("Money")]
    public float money;

    [Header("Food Types")]
    public float hotDogs;
    public float fries;
    public float sandwiches;
    public float lasagne;

    [Header("Food Rates")]
    public float hotDogRate;
    public float friesRate;
    public float sandwichRate;
    public float lasagnaRate;

    [Header("Food Totals")]
    public float totalHotDogs;
    public float totalFries;
    public float totalSandwiches;
    public float totalLasagna;

    public float totalFood =>
        hotDogs + fries + sandwiches + lasagne;
    void Update()
    {
        float dt = Time.deltaTime;

        hotDogs += hotDogRate * dt;
        fries += friesRate * dt;
        sandwiches += sandwichRate * dt;
        lasagne += lasagnaRate * dt;

        totalHotDogs += hotDogRate * dt;
        totalFries += friesRate * dt;
        totalSandwiches += sandwichRate * dt;
        totalLasagna += lasagnaRate * dt;
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }
}