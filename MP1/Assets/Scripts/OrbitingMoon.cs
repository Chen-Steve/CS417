using UnityEngine;

public class OrbitingMoon : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 20f;

    void Update()
    {
        // Rotate around Y axis smoothly, framerate-independent
        transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f, Space.World);
    }
}
