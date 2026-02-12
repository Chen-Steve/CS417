using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class BreakOutTeleport : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference action;

    [Header("Teleport Provider")]
    public TeleportationProvider teleportationProvider;

    [Header("Teleport Targets")]
    public Transform insidePoint;
    public Transform outsidePoint;

    private bool isOutside = false;

    void Start()
    {
        action.action.Enable();
        action.action.started += (ctx) => ToggleTeleport();

        // optional: start inside
        if (insidePoint != null)
        {
            TeleportTo(insidePoint);
            isOutside = false;
        }
    }

    void ToggleTeleport()
    {
        Debug.Log("B pressed - ToggleTeleport fired");
        
        if (insidePoint == null || outsidePoint == null || teleportationProvider == null)
            return;

        Transform target = isOutside ? insidePoint : outsidePoint;
        TeleportTo(target);
        isOutside = !isOutside;
    }

    void TeleportTo(Transform target)
    {
        var req = new TeleportRequest
        {
            destinationPosition = target.position,
            destinationRotation = target.rotation,
            matchOrientation = MatchOrientation.TargetUpAndForward
        };

        teleportationProvider.QueueTeleportRequest(req);
    }
}
