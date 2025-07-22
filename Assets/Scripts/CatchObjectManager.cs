using UnityEngine;
using TMPro;
using Leap;

public class CatchObjectManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject overlay;
    public GameObject spawner;
    public TMP_Text score;
    public TMP_Text time;
    public TMP_Text showHand;
    public TMP_Text highScore;
    public TMP_Text activeScore;
    public TMP_Text activeDeaths;

    [Header("Name Input UI")]
    public GameObject namePromptPanel; // A panel with an input field + button
    public TMP_InputField nameInputField;

    [Header("Leap Motion")]
    public LeapProvider leapProvider;

    [Header("Timer Settings")]
    public float timerDuration = 30f; // Duration in seconds

    private float currentTime;
    private bool timerRunning = false;
    private bool gameActive = false;
    private Hand left;

    void Start()
    {
        LoadHighScore();
        RestartTimer();
    }

    void Update()
    {
        if (!gameActive) {
            Frame frame = leapProvider.CurrentFrame;
            left = frame.GetHand(Chirality.Left);
            if (left != null) {
                Debug.Log("gameActive is true");
                gameActive = true;
                timerRunning = true;
                showHand.text = "";
                spawner.GetComponent<SpawnObjects>().enabled = true;
            }
        } else if (timerRunning)
        {
            currentTime -= Time.deltaTime;

            int seconds = Mathf.CeilToInt(currentTime);
            time.text = $"Time: {Mathf.Max(seconds, 0)}";

            if (currentTime <= 0f)
            {
                EndTimer();
            }
        }
    }

    private void EndTimer()
    {
        timerRunning = false;
        overlay.SetActive(true);

        int newScore = spawner.GetComponent<SpawnObjects>().scoreCount;
        score.text = $"Your score:\n       {newScore}";
        Time.timeScale = 0f;
        spawner.GetComponent<SpawnObjects>().enabled = false;

        // Extract high score
        int oldScore = 0;
        string[] lines = highScore.text.Split('\n');
        if (lines.Length == 2)
        {
            string[] parts = lines[1].Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int parsedScore))
            {
                oldScore = parsedScore;
            }
        }

        if (newScore > oldScore)
        {
            Debug.Log("New high score!");
            namePromptPanel.SetActive(true); // Show name entry UI
        }
    }

    public void SubmitHighScoreName()
    {
        int newScore = spawner.GetComponent<SpawnObjects>().scoreCount;
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            string formatted = $"   High Score:\n{playerName}: {newScore}";
            highScore.text = formatted;

            // Save to PlayerPrefs
            PlayerPrefs.SetString("HighScoreText", formatted);
            PlayerPrefs.SetInt("HighScoreValue", newScore);
            PlayerPrefs.Save();

            namePromptPanel.SetActive(false);
        } else {
            Debug.Log("string not valid");
        }
    }

    public void RestartTimer()
    {
        showHand.text = "Hover your hand over the sensor to start";
        gameActive = false;
        Time.timeScale = 1f;
        currentTime = timerDuration;
        timerRunning = false;
        overlay.SetActive(false);
        spawner.GetComponent<SpawnObjects>().scoreCount = 0;
        spawner.GetComponent<SpawnObjects>().deathCount = 0;

        activeScore.text = "Score: 0";
        activeDeaths.text = "Deaths: 0";
        time.text = $"Time: {timerDuration}";

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("HighScoreText"))
        {
            highScore.text = PlayerPrefs.GetString("HighScoreText");
        }
        else
        {
            highScore.text = "   High Score:\nNone: 0";
        }
    }
}
