using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Playlist")]
    public AudioClip[] songs;
    public float fadeTime = 1.5f;
    public float volume = 0.25f;

    private AudioSource source;
    private int lastIndex = -1;
    private bool firstTrack = true;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.loop = false;
        source.spatialBlend = 0f;

        if (songs == null || songs.Length == 0)
        {
            Debug.LogError("MusicManager: No songs assigned!");
            return;
        }

        StartCoroutine(PlaylistLoop());
    }

    IEnumerator PlaylistLoop()
    {
        while (true)
        {
            int index = GetNextSong();
            yield return StartCoroutine(PlaySong(songs[index]));
        }
    }

    int GetNextSong()
    {
        if (songs.Length == 1) return 0;

        int index;
        do
        {
            index = Random.Range(0, songs.Length);
        } while (index == lastIndex);

        lastIndex = index;
        return index;
    }

    IEnumerator PlaySong(AudioClip clip)
    {
        if (!firstTrack)
            yield return StartCoroutine(FadeOut());

        firstTrack = false;

        source.clip = clip;
        source.Play();

        yield return StartCoroutine(FadeIn());


        while (source.isPlaying)
            yield return null;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        source.volume = 0;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0, volume, t / fadeTime);
            yield return null;
        }

        source.volume = volume;
    }

    IEnumerator FadeOut()
    {
        float start = source.volume;
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(start, 0, t / fadeTime);
            yield return null;
        }

        source.Stop();
    }
}