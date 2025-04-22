using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("PlayerNameInput");  
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }
}

