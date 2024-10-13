using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; 

public class TimerController : MonoBehaviour
{
    public float startTime = 30f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    public TextMeshProUGUI timerText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject gameOverUI;
    public GameObject restartButton;

    public PlayerController playerController;

    public float textDisplayDelay = 5f; 
    public float uiDisplayDelay = 10f;   

    void Start()
    {
        timeRemaining = startTime;
        timerIsRunning = true;

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        gameOverUI.SetActive(false);
        restartButton.SetActive(false); 

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                StartCoroutine(LoseGameWithDelay());
            }
        }

        if (playerController.HasCollectedAllPickups() && timerIsRunning)
        {
            StartCoroutine(WinGameWithDelay());
        }
    }

    private void UpdateTimerDisplay()
    {
        float clampedTime = Mathf.Max(timeRemaining,0);
        float minutes = Mathf.FloorToInt(clampedTime / 60);
        float seconds = Mathf.FloorToInt(clampedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator WinGameWithDelay()
    {
        timerIsRunning = false; 

        winTextObject.SetActive(true); 
        yield return new WaitForSeconds(textDisplayDelay); 

        gameOverUI.SetActive(true); 
        restartButton.SetActive(true); 
    }

    private IEnumerator LoseGameWithDelay()
    {
        timerIsRunning = false; 

        loseTextObject.SetActive(true); 
        yield return new WaitForSeconds(textDisplayDelay); 

        gameOverUI.SetActive(true); 
        restartButton.SetActive(true); 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
