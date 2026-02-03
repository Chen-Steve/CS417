using UnityEngine;
using UnityEngine.InputSystem;

public class BreakOutTeleport : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference action;

    [Header("Teleport Targets")]
    public Transform insidePoint;
    public Transform outsidePoint;

    private bool isOutside = false;

    void Start()
    {
        action.action.Enable();

        // Start inside
        if (insidePoint != null)
        {
            transform.position = insidePoint.position;
            transform.rotation = insidePoint.rotation;
            isOutside = false;
        }

        action.action.started += (ctx) =>
        {
            ToggleTeleport();
        };
    }

    void ToggleTeleport()
    {
        if (insidePoint == null || outsidePoint == null)
            return;

        Transform target = isOutside ? insidePoint : outsidePoint;

        // Move Camera Offset, not XR Origin
        transform.position = target.position;
        transform.rotation = target.rotation;

        isOutside = !isOutside;
    }
}
