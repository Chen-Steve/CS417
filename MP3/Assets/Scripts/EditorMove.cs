using UnityEngine;
using UnityEngine.InputSystem;

public class EditorMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lookSpeed = 100f;

    float yRotation = 0f;

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // WASD movement
        float h = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        float v = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);

        Vector3 move = transform.forward * v + transform.right * h;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Mouse look
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * lookSpeed * Time.deltaTime;
        float mouseY = mouseDelta.y * lookSpeed * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -80f, 80f);

        // rotate player (left/right)
        transform.Rotate(Vector3.up * mouseX);

        // rotate camera (up/down)
        Camera.main.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }
}