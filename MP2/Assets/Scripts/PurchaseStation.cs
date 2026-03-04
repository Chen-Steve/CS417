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

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume=0.5f;

    bool purchased = false;

    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
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

        if (purchased)
        {
            buttonRenderer.material.color = purchasedColor;
            return;
        }

        if (bank != null && bank.money >= cost)
        {
            buttonRenderer.material.color = affordableColor;
        }
        else
        {
            buttonRenderer.material.color = lockedColor;
        }
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (purchased)
            return;

        if (bank == null)
        {
            Debug.LogWarning("Bank not assigned.");
            return;
        }

        if (bank.money >= cost)
        {
            bank.money -= cost;
            stationToEnable.SetActive(true);
            purchased = true;

            interactable.enabled = false;

            Debug.Log("Station purchased!");
        }
        else
        {
            Debug.Log("Not enough money.");
        }

        // play SFX when buying/upgrading station
        audioSource.PlayOneShot(clip, volume);
    }
}