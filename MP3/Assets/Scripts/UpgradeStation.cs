using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// [RequireComponent(typeof(XRSimpleInteractable))]
public class UpgradeStation : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Upgrade Settings")]
    public StationGenerator stationToUpgrade;
    public XRSimpleInteractable upgradeButton;
    public float startingCost = 1f;
    public float costMultiplier = 1.5f;
    public float rateMultiplier = 1.5f;

    [Header("Station Starting Costs")]
    public float hotDogStartingCost = 5f;
    public float friesStartingCost = 10f;
    public float sandwichStartingCost = 20f;
    public float lasagnaStartingCost = 40f;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public TMP_Text costText;
    public TMP_Text cooldownText;
    public Color lockedColor = Color.red;
    public Color affordableColor = Color.green;
    public Color cooldownColor = Color.gray;

    [Header("Cooldown")]
    public float cooldownSeconds = 3f;
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    private int upgradeLevel;
    private float currentCost;
    private float cooldownRemaining;

    void Awake()
    {
        upgradeButton = GetComponent<XRSimpleInteractable>();
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
        upgradeButton.selectEntered.AddListener(OnPressed);
        RefreshButtonState();
        UpdateCooldownText();
    }

    void OnDisable()
    {
        upgradeButton.selectEntered.RemoveListener(OnPressed);
    }

    void Update()
    {
        TickCooldown();
        RefreshButtonState();
        UpdateVisualState();
        UpdateCooldownText();
    }

    void TickCooldown()
    {
        if (cooldownRemaining <= 0f)
            return;

        cooldownRemaining -= Time.deltaTime;

        if (cooldownRemaining < 0f)
            cooldownRemaining = 0f;
    }

    void RefreshButtonState()
    {
        if (upgradeButton == null)
            return;

        bool stationReady = stationToUpgrade != null && stationToUpgrade.gameObject.activeInHierarchy;
        upgradeButton.enabled = stationReady && cooldownRemaining <= 0f;
    }

    void UpdateVisualState()
    {
        if (buttonRenderer == null)
            return;

        if (cooldownRemaining > 0f)
        {
            buttonRenderer.material.color = cooldownColor;
            return;
        }

        if (!(stationToUpgrade != null && stationToUpgrade.gameObject.activeInHierarchy))
        {
            buttonRenderer.material.color = lockedColor;
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

    void UpdateCooldownText()
    {
        if (cooldownText != null)
        {
            if (cooldownRemaining > 0f)
            {
                int secondsLeft = Mathf.CeilToInt(cooldownRemaining);
                cooldownText.text = $"Cooldown: {secondsLeft}s";
            }
            else
            {
                cooldownText.text = string.Empty;
            }

            return;
        }

        if (costText == null)
            return;

        if (cooldownRemaining > 0f)
        {
            int secondsLeft = Mathf.CeilToInt(cooldownRemaining);
            costText.text = $"Cooldown: {secondsLeft}s";
        }
        else
        {
            UpdateCostText();
        }
    }

    void UpdateCostText()
    {
        if (costText == null)
            return;

        costText.text = $"Upgrade Grill Station: ${currentCost:0.##}";
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (bank == null)
        {
            Debug.LogWarning("Bank not assigned.");
            return;
        }

        if (bank.money >= currentCost)
        {
            bank.money -= currentCost;
            stationToUpgrade.MultiplyProductionRate(rateMultiplier);
            upgradeLevel++;
            currentCost *= costMultiplier;
            UpdateCostText();
            cooldownRemaining = Mathf.Max(0f, cooldownSeconds);

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

        RefreshButtonState();
        UpdateCooldownText();
    }
}
