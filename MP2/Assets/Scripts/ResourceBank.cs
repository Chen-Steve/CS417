using UnityEngine;

public class ResourceBank : MonoBehaviour
{
    public float money;

    public void AddMoney(float amount)
    {
        money += amount;
    }
}