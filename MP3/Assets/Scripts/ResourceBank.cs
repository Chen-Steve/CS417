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

    public float totalFood =>
        hotDogs + fries + sandwiches + lasagne;

    void Update()
    {
        float dt = Time.deltaTime;

        hotDogs += hotDogRate * dt;
        fries += friesRate * dt;
        sandwiches += sandwichRate * dt;
        lasagne += lasagnaRate * dt;
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }
}