using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class BalloonClickReward : MonoBehaviour
{
    public ResourceBank bank;
    public float rewardAmount = 20f;

    public AudioClip clip;
    public float volume=0.5f;

    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    
        if (bank == null)
            bank = FindObjectOfType<ResourceBank>();
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnClicked);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnClicked);
    }

    void OnClicked(SelectEnterEventArgs args)
    {
        if (bank != null)
        {
            bank.AddMoney(rewardAmount);
        }

        AudioSource.PlayClipAtPoint(clip, transform.position, volume);

        Destroy(gameObject);
    }
}