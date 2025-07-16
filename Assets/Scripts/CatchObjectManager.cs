using UnityEngine;
using TMPro;
using Leap;

public class CatchObjectManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject spawner;
    public TMP_Text score;
    public TMP_Text time;
    public TMP_Text showHand;

    public TMP_Text activeScore;
    public TMP_Text activeDeaths;

    public LeapProvider leapProvider;

    public float timerDuration = 30f; // Duration in seconds
    private float currentTime;
    private bool timerRunning = false;
    private bool gameActive = false;
    private Hand left;

    void Start()
    {
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
            } else {
                Debug.Log("Left is null");
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
        score.text = $"Your score:\n       {spawner.GetComponent<SpawnObjects>().scoreCount}";
        Time.timeScale = 0f;
        spawner.GetComponent<SpawnObjects>().enabled = false;
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
        time.text = time.text = $"Time: {timerDuration}";

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
