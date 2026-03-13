using TMPro;
using UnityEngine;

public class BankBalanceUI : MonoBehaviour
{
    public ResourceBank bank;
    public TMP_Text balanceText;

    string lastRendered;

    void Start()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        float currentMoney = bank != null ? bank.money : 0f;
        string nextText = $"Balance: ${currentMoney:0}";

        if (nextText == lastRendered)
            return;

        if (balanceText != null)
            balanceText.text = nextText;

        lastRendered = nextText;
    }
}
