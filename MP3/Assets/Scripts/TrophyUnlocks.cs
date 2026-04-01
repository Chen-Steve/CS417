using UnityEngine;

public class TrophyUnlocks : MonoBehaviour
{
    public ResourceBank bank;

    [Header("Trophy Objects")]
    public GameObject hotDogTrophy;
    public GameObject friesTrophy;
    public GameObject sandwichTrophy;
    public GameObject lasagnaTrophy;

    [Header("Unlock Requirements")]
    public float hotDogGoal = 5f;
    public float friesGoal = 5f;
    public float sandwichGoal = 5f;
    public float lasagnaGoal = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    [Header("Particles (placed in scene under trophies)")]
    public ParticleSystem hotDogParticles;
    public ParticleSystem friesParticles;
    public ParticleSystem sandwichParticles;
    public ParticleSystem lasagnaParticles;

    void Start()
    {
    }

    void Update()
    {        
        if (bank == null)
            return;

        if (bank.totalHotDogs >= hotDogGoal && !IsTrophyActive(hotDogTrophy))
        {
            SetTrophyActive(hotDogTrophy);
            SpawnParticles(hotDogParticles);
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip, volume);
        }

        if (bank.totalFries >= friesGoal && !IsTrophyActive(friesTrophy))
        {
            SetTrophyActive(friesTrophy);
            SpawnParticles(friesParticles);
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip, volume);
        }

        if (bank.totalSandwiches >= sandwichGoal && !IsTrophyActive(sandwichTrophy))
        {
            SetTrophyActive(sandwichTrophy);
            SpawnParticles(sandwichParticles);
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip, volume);
        }

        if (bank.totalLasagna >= lasagnaGoal && !IsTrophyActive(lasagnaTrophy))
        {
            SetTrophyActive(lasagnaTrophy);
            SpawnParticles(lasagnaParticles);
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip, volume);
        }
    }

    void SetTrophyActive(GameObject trophy)
    {
        if (trophy == null)
            return;

        trophy.SetActive(true);
    }

    bool IsTrophyActive(GameObject trophy)
    {
        return trophy != null && trophy.activeSelf;
    }

    void SpawnParticles(ParticleSystem particles)
    {
        if (particles == null)
            return;

        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particles.Play();
    }
}
