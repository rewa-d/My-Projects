using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField player1InputField;  
    public TMP_Text errorMessageText;         

    
    public void OnStartGameButtonClicked()
    {
        string player1Name = player1InputField.text;

        if (!string.IsNullOrEmpty(player1Name))
        {
            PlayerPrefs.SetString("Player1Name", player1Name);
            PlayerPrefs.SetString("Player2Name", "AI");
            SceneManager.LoadScene("SquareBoardGame");
        }
        else
        {
            errorMessageText.text = "Please enter Player 1's name.";
        }
    }
}
