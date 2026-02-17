using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int keysNeeded = 3;
    public Transform door;                 // drag door_body here
    public Vector3 openOffset = new Vector3(0f, 90f, 0f);
    public float openSpeed = 2f;

    private bool opening = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        Debug.Log("DoorController START on: " + gameObject.name);

        if (door == null)
        {
            Debug.LogError("DoorController: 'door' is NOT assigned!");
            return;
        }

        closedRot = door.localRotation;
        openRot = closedRot * Quaternion.Euler(openOffset);

        Debug.Log($"DoorController: keysNeeded={keysNeeded}, door={door.name}, openOffset={openOffset}");

        if (keysNeeded == 0)
        {
            opening = true;
            Debug.Log("DoorController: TEST MODE opening = true");
        }
    }

    void Update()
    {
        if (!opening) return;
        door.localRotation = Quaternion.Slerp(door.localRotation, openRot, Time.deltaTime * openSpeed);
    }

    public void RegisterKey()
    {
        opening = true;
        Debug.Log("DoorController: RegisterKey called -> opening true");
    }
}
