using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public InputActionReference action;

    private Light pointLight;
    private bool isOn = true;

    void Start()
    {
        // Get the Light component from this GameObject
        pointLight = GetComponent<Light>();

        // Enable input
        action.action.Enable();

        // When button is pressed
        action.action.performed += (ctx) =>
        {
            Debug.Log("Light Switch Pressed");
            ToggleLight();
        };

    }

    void ToggleLight()
    {
        if (isOn)
        {
            pointLight.color = Color.purple;   // change color
            isOn = false;
        }
        else
        {
            pointLight.color = Color.white; // back to normal
            isOn = true;
        }
    }
}
