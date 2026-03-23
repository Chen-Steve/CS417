using UnityEngine;

public class AutoDestroyFood : MonoBehaviour
{
    public float lifetime = 15f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}