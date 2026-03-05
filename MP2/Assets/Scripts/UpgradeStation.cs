using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class UpgradeStation : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Upgrade Settings")]
    public StationGenerator stationToUpgrade;
    public float startingCost = 1f;
    public float costMultiplier = 1.5f;
    public float rateMultiplier = 1.5f;

    [Header("Per-Station Starting Costs")]
    public float hotDogStartingCost = 5f;
    public float friesStartingCost = 10f;
    public float sandwichStartingCost = 20f;
    public float lasagnaStartingCost = 40f;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public TMP_Text costText;
    private string costLabel = "Upgrade Grill Station";

    public Color lockedColor = Color.red;
    public Color affordableColor = Color.green;
    public Color upgradedColor = Color.gray;

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    public int upgradeLevel;

    float currentCost;

    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        currentCost = Mathf.Max(0f, GetInitialCost());
        UpdateCostText();
    }

    float GetInitialCost()
    {
        if (stationToUpgrade == null)
            return startingCost;

        switch (stationToUpgrade.produces)
        {
            case StationGenerator.FoodType.HotDog:
                return hotDogStartingCost;
            case StationGenerator.FoodType.Fries:
                return friesStartingCost;
            case StationGenerator.FoodType.Sandwich:
                return sandwichStartingCost;
            case StationGenerator.FoodType.Lasagna:
                return lasagnaStartingCost;
            default:
                return startingCost;
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }

    void Update()
    {
        UpdateVisualState();
    }

    void UpdateVisualState()
    {
        if (buttonRenderer == null)
            return;

        if (!(stationToUpgrade != null && stationToUpgrade.gameObject.activeInHierarchy))
        {
            buttonRenderer.material.color = upgradedColor;
            return;
        }

        if (bank != null && bank.money >= currentCost)
        {
            buttonRenderer.material.color = affordableColor;
        }
        else
        {
            buttonRenderer.material.color = lockedColor;
        }
    }

    void UpdateCostText()
    {
        if (costText == null)
            return;

        costText.text = $"{costLabel}: ${currentCost:0.##}";
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (bank == null)
        {
            Debug.LogWarning("Bank not assigned.");
            return;
        }

        if (stationToUpgrade == null)
        {
            Debug.LogWarning("Station to upgrade not assigned.");
            return;
        }

        if (!(stationToUpgrade != null && stationToUpgrade.gameObject.activeInHierarchy))
        {
            Debug.Log("Station not purchased yet.");
            return;
        }

        if (rateMultiplier <= 1f)
        {
            Debug.LogWarning("Rate multiplier must be greater than 1.");
            return;
        }

        if (costMultiplier <= 1f)
        {
            Debug.LogWarning("Cost multiplier must be greater than 1.");
            return;
        }

        if (bank.money >= currentCost)
        {
            bank.money -= currentCost;
            stationToUpgrade.MultiplyProductionRate(rateMultiplier);
            upgradeLevel++;
            currentCost *= costMultiplier;
            UpdateCostText();

            Debug.Log($"Station upgraded to level {upgradeLevel}. Next cost: {currentCost:0.##}");

            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip, volume);
            }
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }
}
