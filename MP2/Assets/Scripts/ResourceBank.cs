using UnityEngine;
using TMPro;

public class ResourceBank : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "$ " + Mathf.FloorToInt(money).ToString();
    }
}