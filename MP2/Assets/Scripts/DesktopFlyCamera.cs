using UnityEngine;

public class DesktopFlyCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float fastMultiplier = 3f;
    public float slowMultiplier = 0.35f;

    [Header("Look")]
    public float lookSensitivity = 2f;
    public bool lockCursor = true;

    float yaw;
    float pitch;

    void OnEnable()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isLocked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isLocked;
        }

        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

       
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= fastMultiplier;
        if (Input.GetKey(KeyCode.LeftControl)) speed *= slowMultiplier;

        Vector3 input =
            (transform.forward * GetAxisRaw(KeyCode.W, KeyCode.S)) +
            (transform.right   * GetAxisRaw(KeyCode.D, KeyCode.A)) +
            (transform.up      * GetAxisRaw(KeyCode.E, KeyCode.Q));

        transform.position += input.normalized * speed * Time.unscaledDeltaTime;
    }

    static float GetAxisRaw(KeyCode positive, KeyCode negative)
    {
        float v = 0f;
        if (Input.GetKey(positive)) v += 1f;
        if (Input.GetKey(negative)) v -= 1f;
        return v;
    }
}