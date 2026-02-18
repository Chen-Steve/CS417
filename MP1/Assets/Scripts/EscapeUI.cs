using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EscapeUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;

    public int locksNeeded = 3;
    private int locksTriggered = 0;

    public float timeRemaining = 180f;
    private bool lost = false;

    void Start()
    {
        UpdateScore();
    }

    void Update()
    {
        if (lost) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            lost = true;
            timerText.text = "TIME: 00:00\nYOU LOSE";
            return;
        }

        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Time: {min:00}:{sec:00}";
    }

    public void SetLocksTriggered(int value)
    {
        locksTriggered = value;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = $"Locks: {locksTriggered}/{locksNeeded}";
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
