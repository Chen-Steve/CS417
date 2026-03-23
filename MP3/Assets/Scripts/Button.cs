using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialButton : MonoBehaviour
{
    public TutorialManager tutorial;

    void OnEnable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().activated.AddListener(OnPressed);
    }

    void OnDisable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().activated.RemoveListener(OnPressed);
    }

    void OnPressed(ActivateEventArgs args)
    {
        tutorial.ClosePopup();
    }
}