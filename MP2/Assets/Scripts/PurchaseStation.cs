using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class PurchaseStation : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Station Settings")]
    public GameObject stationToEnable;
    public float cost = 50f;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public Color lockedColor = Color.red;
    public Color affordableColor = Color.green;
    public Color purchasedColor = Color.gray;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    [Header("Tutorial")]
    public TutorialManager tutorial;

    bool purchased = false;
    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void Start()
    {
        if (stationToEnable != null)
            stationToEnable.SetActive(false);

        UpdateVisualState();
    }

    void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        if (interactable != null)
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

        if (purchased)
        {
            buttonRenderer.material.color = purchasedColor;
            return;
        }

        if (bank != null && bank.money >= cost)
            buttonRenderer.material.color = affordableColor;
        else
            buttonRenderer.material.color = lockedColor;
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (purchased)
            return;

        if (bank == null)
        {
            Debug.LogWarning("PurchaseStation: Bank not assigned.");
            return;
        }

        if (stationToEnable == null)
        {
            Debug.LogWarning("PurchaseStation: stationToEnable not assigned.");
            return;
        }

        if (bank.money < cost)
        {
            Debug.Log("Not enough money.");
            return;
        }

        bank.money -= cost;
        stationToEnable.SetActive(true);
        purchased = true;

        if (tutorial != null)
            tutorial.OnFirstStationPurchased();

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);

        interactable.enabled = false;

        UpdateVisualState();

        Debug.Log("Station purchased!");
    }
}