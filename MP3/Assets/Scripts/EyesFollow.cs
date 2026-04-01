using UnityEngine;

public class PupilFollow : MonoBehaviour
{
    public Transform eyeCenter;
    public Transform player;   // XR Origin or Main Camera

    public float radius = 0.15f;

    void Update()
    {
        if (eyeCenter == null || player == null) return;

        // direction from eye → player
        Vector3 dir = (player.position - eyeCenter.position).normalized;

        // place pupil on sphere surface
        transform.position = eyeCenter.position + dir * radius;
    }
}