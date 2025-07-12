using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject ledHolder;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            if (pauseUI != null)
                pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            if (pauseUI != null)
                pauseUI.SetActive(false);
        }
    }

    void OnDisable()
    {
        // Ensure time scale resets when the object is destroyed or script disabled
        Time.timeScale = 1f;
    }
    public void loadFloatingObject()
    {
        SceneManager.LoadScene("HandDemo", LoadSceneMode.Single);
    }

    public void loadCatchObject()
    {
        SceneManager.LoadScene("CatchObject", LoadSceneMode.Single);
    }

    public void loadPaint()
    {
        SceneManager.LoadScene("Paint", LoadSceneMode.Single);
    }
    public void loadSendHand()
    {
        SceneManager.LoadScene("SendHandInfo", LoadSceneMode.Single);
    }

    public void ClearLEDS()
    {
        int i = 0;
        foreach (Transform child in ledHolder.transform)
        {
            if (i > 0) child.GetComponent<SendCollision>().exit = true;
            i++;
        }
    }
}
