using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public ResourceBank bank;

    [Header("UI")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public float popupAnimationDuration = 0.5f;

    int stage = 0;
    bool waitingForStationPurchase = false;

    void Start()
    {
        ShowPopup1();
    }

    void Update()
    {
        if (stage == 1 && bank != null && bank.money >= 10f)
        {
            ShowPopup2();
        }
    }

    public void ClosePopup()
    {
        if (tutorialPanel != null)
            StartCoroutine(ClosePopupAnimation());

        if (stage == 3)
        {
            ShowPopup4();
        }
    }

    void ShowPopup1()
    {
        stage = 1;

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            StartCoroutine(ShowPopupAnimation());
        }

        if (tutorialText != null)
        {
            tutorialText.text =
                "Welcome to Goblin Gourmet!\n\n" +
                "Help run a restaurant with friendly goblins and make your restaurant famous!\n\n" +
                "First, we need funding; click on the ATM to earn some money.";
        }
    }

    void ShowPopup2()
    {
        stage = 2;
        waitingForStationPurchase = true;

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            StartCoroutine(ShowPopupAnimation());
        }

        if (tutorialText != null)
        {
            tutorialText.text =
                "Great! Now we have enough for our first cooking station.\n\n" +
                "Go over to the signboard and click the green button to buy it.";
        }
    }

    public void OnFirstStationPurchased()
    {
        if (!waitingForStationPurchase)
            return;

        waitingForStationPurchase = false;
        stage = 3;

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            StartCoroutine(ShowPopupAnimation());
        }

        if (tutorialText != null)
        {
            tutorialText.text =
                "Nice! Your goblin chef will keep cooking at this station, and customers will be drawn in when the food is cooked, earning you more money!\n\n" +
                "To speed up the chef's cooking, you can upgrade the station.";
        }
    }

    void ShowPopup4()
    {
        stage = 4;

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            StartCoroutine(ShowPopupAnimation());
        }

        if (tutorialText != null)
        {
            tutorialText.text =
                "Congratulations, you are on the way to becoming an aspiring restaurant owner!\n\n" +
                "If you make enough food, your achievements will be recognized at the large table.\n\n" +
                "Happy cooking!";
        }
    }

    IEnumerator ClosePopupAnimation()
    {
        CanvasGroup canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        float originalAlpha = canvasGroup.alpha;

        float t = 0f;
        while (t < popupAnimationDuration && canvasGroup.alpha > 0f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(1f, 0f, t/popupAnimationDuration);
            yield return null;
        }

        tutorialPanel.SetActive(false);
        canvasGroup.alpha = originalAlpha;
    }
    IEnumerator ShowPopupAnimation()
    {
        CanvasGroup canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        float t = 0f;
        while (t < popupAnimationDuration && canvasGroup.alpha < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, t/popupAnimationDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}