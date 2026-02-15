using UnityEngine;

public class LampToggle : MonoBehaviour
{
    public GameObject secretMessage; 
    private bool isOn;

    public void Toggle()
    {
        isOn = !isOn;

        SendMessage(isOn ? "turnOnAll" : "turnOffAll", SendMessageOptions.DontRequireReceiver);

        if (secretMessage != null)
            secretMessage.SetActive(isOn);
    }
}
