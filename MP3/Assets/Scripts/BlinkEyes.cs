using UnityEngine;
using System.Collections;

public class BlinkEyes : MonoBehaviour
{
    public GameObject eyes;

    void Start()
    {
        InvokeRepeating(nameof(Blink), 2f, 3f);
    }

    void Blink()
    {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        eyes.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        eyes.SetActive(true);
    }
}