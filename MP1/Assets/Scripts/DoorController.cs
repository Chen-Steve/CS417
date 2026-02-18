using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Unlock Requirements")]
    public int locksNeeded = 3;                 // number of sockets/locks (usually 3)

    [Header("Transform that actually moves (door hinge / moving door part)")]
    public Transform door;

    [Header("Open Rotation (degrees)")]
    public Vector3 openOffset = new Vector3(0f, 90f, 0f);
    public float openSpeed = 2f;

    [Header("Safety")]
    public float ignoreEventsForSeconds = 1.0f; // ignore early socket noise
    public bool holdDoorClosedUntilUnlocked = true; // IMPORTANT: prevents auto-opening from other components

    private bool[] lockTriggered;
    private int triggeredCount = 0;

    private bool unlocked = false;

    private Quaternion closedRot;
    private Quaternion openRot;

    private float startTime;

    void Awake()
    {
        startTime = Time.time;

        if (door == null)
        {
            Debug.LogError("DoorController: 'door' Transform not assigned.");
            enabled = false;
            return;
        }

        closedRot = door.localRotation;
        openRot = closedRot * Quaternion.Euler(openOffset);

        lockTriggered = new bool[Mathf.Max(1, locksNeeded)];
        triggeredCount = 0;
        unlocked = false;

        door.localRotation = closedRot;

        Debug.Log($"DoorController ready. locksNeeded={locksNeeded}. Holding closed={holdDoorClosedUntilUnlocked}");
    }

    void Update()
    {
        if (!unlocked)
        {
            // Clamp door shut unless unlocked
            if (holdDoorClosedUntilUnlocked)
                door.localRotation = closedRot;

            return;
        }

        door.localRotation = Quaternion.Slerp(door.localRotation, openRot, Time.deltaTime * openSpeed);
    }

    public void RegisterKeyFromSocket(int socketIndex)
    {
        if (Time.time - startTime < ignoreEventsForSeconds)
            return;

        if (socketIndex < 0 || socketIndex >= lockTriggered.Length)
        {
            Debug.LogWarning($"DoorController: socketIndex {socketIndex} out of range (0..{lockTriggered.Length - 1})");
            return;
        }

        if (lockTriggered[socketIndex])
            return; // don’t double count

        lockTriggered[socketIndex] = true;
        triggeredCount++;

        Debug.Log($"DoorController: lock {socketIndex} triggered -> {triggeredCount}/{locksNeeded}");

        if (triggeredCount >= locksNeeded)
        {
            unlocked = true;
            Debug.Log("DoorController: UNLOCKED -> opening door");
        }
    }
}
