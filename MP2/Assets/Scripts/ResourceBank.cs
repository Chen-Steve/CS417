using UnityEngine;
using TMPro;

public class ResourceBank : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyText;

    void Start()
    {
    }

    public void AddMoney(float amount)
    {
        money += amount;
        Debug.Log("Money added: " + amount + ". Total: " + money);
    }
}