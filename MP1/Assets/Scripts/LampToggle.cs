using UnityEngine;

public class LampToggle : MonoBehaviour
{
    bool isOn;

    public void Toggle()
    {
        isOn = !isOn;
        SendMessage(isOn ? "turnOnAll" : "turnOffAll", SendMessageOptions.DontRequireReceiver);
    }
}
