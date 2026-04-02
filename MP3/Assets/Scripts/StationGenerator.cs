using UnityEngine;
using System.Collections.Generic;

public class StationGenerator : MonoBehaviour
{
    public ResourceBank bank;

    public enum FoodType
    {
        HotDog,
        Fries,
        Sandwich,
        Lasagna
    }

    [Header("Visual Spawn Settings")]
    public FoodType produces;
    public float foodPerSecond = 1f;
    public Transform spawnPoint;
    public int maxVisualFood = 3;
    public GameObject foodPrefab;
    public float visualLifetime = 15f;
    public Transform customerStandPoint;
    readonly List<GameObject> activeVisuals = new List<GameObject>();
    float visualSpawnProgress;
    public ParticleSystem foodParticles;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    bool isActive = false;

    void OnEnable()
    {
        StartGenerating();
    }

    void StartGenerating()
    {
        if (bank == null || isActive)
            return;

        ApplyRateDelta(foodPerSecond);

        isActive = true;
    }

    void OnDisable()
    {
        if (bank == null || !isActive)
            return;

        ApplyRateDelta(-foodPerSecond);
        isActive = false;
    }

    public void IncreaseProductionRate(float amount)
    {
        if (amount <= 0f)
            return;

        SetProductionRate(foodPerSecond + amount);
    }

    public void SetProductionRate(float newRate)
    {
        float clampedRate = Mathf.Max(0f, newRate);

        if (Mathf.Approximately(clampedRate, foodPerSecond))
            return;

        if (bank != null && isActive)
        {
            float delta = clampedRate - foodPerSecond;
            ApplyRateDelta(delta);
        }

        foodPerSecond = clampedRate;
    }

    public void MultiplyProductionRate(float multiplier)
    {
        if (multiplier <= 0f)
            return;

        SetProductionRate(foodPerSecond * multiplier);
    }

    void ApplyRateDelta(float delta)
    {
        switch (produces)
        {
            case FoodType.HotDog:
                bank.hotDogRate += delta;
                break;

            case FoodType.Fries:
                bank.friesRate += delta;
                break;

            case FoodType.Sandwich:
                bank.sandwichRate += delta;
                break;

            case FoodType.Lasagna:
                bank.lasagnaRate += delta;
                break;
        }
    }

    void Update()
    {
        if (bank == null || !isActive)
            return;

        visualSpawnProgress += foodPerSecond * Time.deltaTime;

        while (visualSpawnProgress >= 1f)
        {
            if (!SpawnFood())
            {
                break;
            }

            visualSpawnProgress -= 1f;
        }
    }

    bool SpawnFood()
    {
        if (foodPrefab == null || spawnPoint == null)
            return false;

        activeVisuals.RemoveAll(visual => visual == null);

        if (maxVisualFood > 0 && activeVisuals.Count >= maxVisualFood)
        {
            GameObject oldest = activeVisuals[0];
            activeVisuals.RemoveAt(0);

            if (oldest != null)
                Destroy(oldest);
        }

        GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);
        EnableSpawnedFoodPhysics(food);
        activeVisuals.Add(food);
        PlayFoodParticles();

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);

        // when destroyed, reduce counter
        StartCoroutine(DestroyVisualAfterDelay(food, visualLifetime));

        return true;
    }

    // coroutine
    System.Collections.IEnumerator DestroyVisualAfterDelay(GameObject food, float delay)
    {
        yield return new WaitForSeconds(delay);

        activeVisuals.Remove(food);

        if (food != null)
            Destroy(food);
    }

    void EnableSpawnedFoodPhysics(GameObject food)
    {
        if (food == null)
            return;

        Rigidbody[] bodies = food.GetComponentsInChildren<Rigidbody>(true);
        for (int i = 0; i < bodies.Length; i++)
        {
            Rigidbody body = bodies[i];
            if (body == null)
                continue;

            body.isKinematic = false;
            body.useGravity = true;
            body.WakeUp();
        }
    }

    void PlayFoodParticles()
    {
        if (foodParticles == null || spawnPoint == null)
            return;

        foodParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        foodParticles.Play();

        // foodParticles.transform.position = spawnPoint.position;
        // foodParticles.transform.rotation = spawnPoint.rotation;
    }
}