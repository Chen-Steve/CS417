using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ATMClicker : MonoBehaviour
{
    public int dollarsPerHit = 1;
    public ResourceBank bank;

    void OnEnable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().activated.AddListener(OnActivated);
    }

    void OnDisable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().activated.RemoveListener(OnActivated);
    }

    void OnActivated(ActivateEventArgs args)
    {
        if (bank != null)
            bank.AddMoney(dollarsPerHit);
    }
}