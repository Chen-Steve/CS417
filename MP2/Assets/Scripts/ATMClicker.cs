using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ATMClicker : MonoBehaviour
{
    public int dollarsPerHit = 1;
    public ResourceBank bank;

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume=0.5f;

    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("ATMClicker: selectEntered fired");
        AddMoney();
    }

    void AddMoney()
    {
        if (bank != null) {
            Debug.Log("Adding money to bank: " + bank.name);
            bank.AddMoney(dollarsPerHit);
        }
        else
            Debug.LogWarning("ATMClicker: Bank is not assigned.");
        
        // play SFX for each add-money click
        audioSource.PlayOneShot(clip, volume);
    }
}