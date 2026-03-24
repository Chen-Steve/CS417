using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class PurchaseStation : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Station Settings")]
    public GameObject stationToEnable;
    public float cost = 50f;

    [Header("Particle Spawn (set this on the station)")]
    public Transform particleSpawnPoint;
    public ParticleSystem purchaseParticlesPrefab;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public Color lockedColor = Color.red;
    public Color affordableColor = Color.green;
    public Color purchasedColor = Color.gray;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    [Header("Tutorial")]
    public TutorialManager tutorial;

    [Header("Animation")]
    public float scaleDuration = 2f;
    public float overshootAmount = 1.1f;

    bool purchased = false;
    XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void Start()
    {
        if (stationToEnable != null)
        {
            stationToEnable.SetActive(false);
            stationToEnable.transform.localScale = Vector3.one * 0.1f;
        }

        UpdateVisualState();
    }

    void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnPressed);
    }

    void Update()
    {
        UpdateVisualState();
    }

    void UpdateVisualState()
    {
        if (buttonRenderer == null)
            return;

        if (purchased)
        {
            buttonRenderer.material.color = purchasedColor;
            return;
        }

        if (bank != null && bank.money >= cost)
            buttonRenderer.material.color = affordableColor;
        else
            buttonRenderer.material.color = lockedColor;
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (purchased)
            return;

        if (bank == null || stationToEnable == null)
            return;

        if (bank.money < cost)
            return;

        bank.money -= cost;

        stationToEnable.SetActive(true);

        // animation
        StartCoroutine(ScaleIn(stationToEnable));

        // particles
        SpawnParticles();

        purchased = true;

        if (tutorial != null)
            tutorial.OnFirstStationPurchased();

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);

        interactable.enabled = false;

        UpdateVisualState();
    }

    void SpawnParticles()
    {
        if (purchaseParticlesPrefab == null || particleSpawnPoint == null)
            return;

        // EXACT position + rotation of spawn point
        ParticleSystem ps = Instantiate(
            purchaseParticlesPrefab,
            particleSpawnPoint.position,
            particleSpawnPoint.rotation
        );

        ps.Play();

        float lifetime = ps.main.duration + ps.main.startLifetime.constantMax;
        Destroy(ps.gameObject, lifetime);
    }

    IEnumerator ScaleIn(GameObject target)
    {
        float time = 0f;

        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;

        target.transform.localScale = start;

        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            float t = time / scaleDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            Vector3 scale;

            if (overshootAmount > 1f)
            {
                float overshootT = Mathf.Sin(t * Mathf.PI * 0.5f);
                scale = Vector3.LerpUnclamped(start, end * overshootAmount, overshootT);

                if (t > 0.8f)
                {
                    float settleT = (t - 0.8f) / 0.2f;
                    scale = Vector3.Lerp(scale, end, settleT);
                }
            }
            else
            {
                scale = Vector3.Lerp(start, end, t);
            }

            target.transform.localScale = scale;
            yield return null;
        }

        target.transform.localScale = end;
    }
}