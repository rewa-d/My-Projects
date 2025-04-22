using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text player1NameText; 
    public TMP_Text player2NameText;

    void Start()
    {
        string player1Name = PlayerPrefs.GetString("Player1Name", "Player 1");
        string player2Name = PlayerPrefs.GetString("Player2Name", "AI");

        player1NameText.text = player1Name;
        player2NameText.text = player2Name;
    }
}
