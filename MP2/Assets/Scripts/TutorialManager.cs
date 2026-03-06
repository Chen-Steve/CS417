using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public ResourceBank bank;

    [Header("UI")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    int stage = 0;

    bool waitingForStationPurchase = false;

    void Start()
    {
        ShowPopup1();
    }

    void Update()
    {
        if (stage == 1 && bank.money >= 10)
        {
            ShowPopup2();
        }
    }

    public void ClosePopup()
    {
        tutorialPanel.SetActive(false);

        if (stage == 2)
        {
            ShowPopup4();
        }
    }

    void ShowPopup1()
    {
        stage = 1;

        tutorialPanel.SetActive(true);

        tutorialText.text =
        "Welcome to Goblin Gourmet!\n\n" +
        "Help run a restaurant with friendly goblins and make your restaurant famous!\n\n" +
        "First, we need funding; click on the ATM to earn some money.";
    }

    void ShowPopup2()
    {
        stage = 2;

        tutorialPanel.SetActive(true);

        tutorialText.text =
        "Great! Now we have enough for our first cooking station.\n\n" +
        "Go over to the signboard and click the green button to buy it.";
    }

    public void OnFirstStationPurchased()
    {
        if (stage != 2) return;

        tutorialPanel.SetActive(true);

        tutorialText.text =
        "Nice! Your goblin chef will keep cooking at this station, and customers will be drawn in when the food is cooked, earning you more money!\n\n" +
        "To speed up the chef's cooking, you can upgrade the station.";
    }

    void ShowPopup4()
    {
        stage = 4;

        tutorialPanel.SetActive(true);

        tutorialText.text =
        "Congratulations, you are on the way to becoming an aspiring restaurant owner!\n\n" +
        "If you make enough food, your achievements will be recognized at the large table.\n\n" +
        "Happy cooking!";
    }
}