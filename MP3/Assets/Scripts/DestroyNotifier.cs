using UnityEngine;
using System;

public class DestroyNotifier : MonoBehaviour
{
    public Action OnDestroyed;

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}