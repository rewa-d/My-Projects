using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    public GameObject splashScreen;
    
    public float displayTime = 3f; 
    private float timer;

    void Start()
    {
        splashScreen.SetActive(true);
        timer = displayTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            LoadMainMenu();
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");    
    }
}
