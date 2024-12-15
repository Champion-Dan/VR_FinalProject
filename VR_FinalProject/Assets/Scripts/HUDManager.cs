using System.Collections;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private int score = 0;
    [SerializeField] private float timer = 180f; // Timer adjustable in the Inspector
    private bool isTimerRunning = false; // Game starts only after hitting the cube
    private bool musicStarted = false;
    private bool gameEnded = false; // Flag to prevent score updates after time ends

    private AudioSource audioSource;
    public AudioClip endSound; // Sound to play when the timer runs out

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource component found. Please add one.");
        }
    }

    public void IncreaseScore(int points)
    {
        if (isTimerRunning && !gameEnded) // Only allow scoring if the game is running
        {
            score += points;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator TimerCountdown()
    {
        while (isTimerRunning && timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();

            if (timer <= 15f && !musicStarted)
            {
                StartMusic();
            }

            yield return null;
        }

        if (timer <= 0)
        {
            timer = 0;
            EndGame();
        }
    }

    private void StartMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            musicStarted = true;
            Debug.Log("Music started at 15 seconds remaining!");
        }
    }

    private void EndGame()
    {
        isTimerRunning = false;
        gameEnded = true; // Mark the game as ended to prevent score updates

        // Play the end sound
        if (audioSource != null && endSound != null)
        {
            audioSource.PlayOneShot(endSound);
            Debug.Log("End sound played!");
        }

        Debug.Log("Time's up! Final score: " + score);
    }

    public void StartGame()
    {
        if (!isTimerRunning) // Start the game only once
        {
            isTimerRunning = true;
            StartCoroutine(TimerCountdown());
            Debug.Log("Game started!");
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timer = 180f; // Reset the timer to its default or configured value
        isTimerRunning = false;
        gameEnded = false; // Allow the score to update again
        musicStarted = false;
        StartCoroutine(TimerCountdown());
    }
}
