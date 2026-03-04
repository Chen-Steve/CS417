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
    public Color purchasedColor = Color.green;

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

            // Change color (doesn't seem to work)
            if (buttonRenderer != null)
                buttonRenderer.material.color = purchasedColor;

            // Disable interaction
            interactable.enabled = false;

            Debug.Log("Station purchased!");
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }
}