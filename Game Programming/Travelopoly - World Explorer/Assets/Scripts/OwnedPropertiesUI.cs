using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class OwnedPropertiesUI : MonoBehaviour
{
    public GameObject propertyCardPrefab;

    public GameObject scrollViewPlayer1;
    public Transform contentPanelPlayer1;

    public GameObject scrollViewPlayer2;
    public Transform contentPanelPlayer2;

    public Button player1Button;
    public Button player2Button;
    public TMP_Text player1ButtonText;
    public TMP_Text player2ButtonText;

    private DatabaseManager dbManager;
    private bool isPlayer1ViewVisible = false;
    private bool isPlayer2ViewVisible = false;

    void Start()
    {
        dbManager = FindObjectOfType<DatabaseManager>();

        if (dbManager == null)
            Debug.LogError("dbManager is NULL!");
        else
            Debug.Log("dbManager found.");

        scrollViewPlayer1.SetActive(false);
        scrollViewPlayer2.SetActive(false);

        player1ButtonText.text = "Show Player 1 Properties";
        player2ButtonText.text = "Show Player 2 Properties";

        player1Button.onClick.AddListener(() => TogglePlayerProperties(1));
        player2Button.onClick.AddListener(() => TogglePlayerProperties(2));
    }

    void TogglePlayerProperties(int playerId)
    {
        if (playerId == 1)
        {
            if (isPlayer1ViewVisible)
            {
                scrollViewPlayer1.SetActive(false);
                isPlayer1ViewVisible = false;
                player1ButtonText.text = "Show Player 1 Properties";
            }
            else
            {
                ShowOwnedProperties(1, contentPanelPlayer1);
                scrollViewPlayer1.SetActive(true);
                scrollViewPlayer2.SetActive(false);
                isPlayer1ViewVisible = true;
                isPlayer2ViewVisible = false;
                player1ButtonText.text = "Hide Player 1 Properties";
                player2ButtonText.text = "Show Player 2 Properties";
            }
        }
        else if (playerId == 2)
        {
            if (isPlayer2ViewVisible)
            {
                scrollViewPlayer2.SetActive(false);
                isPlayer2ViewVisible = false;
                player2ButtonText.text = "Show Player 2 Properties";
            }
            else
            {
                ShowOwnedProperties(2, contentPanelPlayer2);
                scrollViewPlayer2.SetActive(true);
                scrollViewPlayer1.SetActive(false);
                isPlayer2ViewVisible = true;
                isPlayer1ViewVisible = false;
                player2ButtonText.text = "Hide Player 2 Properties";
                player1ButtonText.text = "Show Player 1 Properties";
            }
        }
    }

    void ShowOwnedProperties(int playerId, Transform contentPanel)
    {
        if (dbManager == null || contentPanel == null)
        {
            Debug.LogError($"dbManager or contentPanel is NULL for Player {playerId}!");
            return;
        }

        ClearPropertyList(contentPanel);

        List<int> ownedPositions = dbManager.GetPlayerOwnedProperties(playerId);

        if (ownedPositions.Count == 0)
        {
            Debug.Log($"Player {playerId} owns no properties.");
            return;
        }

        List<PropertyData> ownedProperties = ownedPositions
            .Select(pos => dbManager.GetPropertyDetailsByPosition(pos))
            .Where(p => p != null)
            .OrderBy(p => p.ColorCategory)
            .ToList();

        Debug.Log($"Player {playerId} owns {ownedProperties.Count} properties.");

        foreach (var property in ownedProperties)
        {
            GameObject card = Instantiate(propertyCardPrefab, contentPanel);

            Sprite sprite = Resources.Load<Sprite>($"PropertyCards/position_{property.Position}");
            if (sprite != null)
                card.GetComponent<Image>().sprite = sprite;
            else
                Debug.LogWarning($"Image not found for {property.Name} at position_{property.Position}");
        }
    }

    void ClearPropertyList(Transform contentPanel)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshOwnedProperties(int playerId)
    {
        if (playerId == 1 && isPlayer1ViewVisible)
        {
            ShowOwnedProperties(1, contentPanelPlayer1);
        }
        else if (playerId == 2 && isPlayer2ViewVisible)
        {
            ShowOwnedProperties(2, contentPanelPlayer2);
        }
    }
}
