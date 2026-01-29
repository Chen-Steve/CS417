using UnityEngine;
using UnityEngine.InputSystem;

public class BreakOutTeleport : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference action;

    [Header("Teleport Targets")]
    public Transform insidePoint;   // where player starts
    public Transform outsidePoint;  // external viewing point

    private bool isOutside = false;

    void Start()
    {
        // Enable input
        action.action.Enable();

        // Teleport on button press
        action.action.performed += (ctx) =>
        {
            ToggleTeleport();
        };
    }

    void ToggleTeleport()
    {
        Transform target = isOutside ? insidePoint : outsidePoint;

        // Move player rig root to target position/rotation
        transform.SetPositionAndRotation(target.position, target.rotation);

        isOutside = !isOutside;
    }
}
