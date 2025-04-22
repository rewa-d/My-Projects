using UnityEngine;
using System.Collections;

public class PlayerTokenMovement : MonoBehaviour
{
    public Transform[] boardSpaces;
    public float moveSpeed = 5f;
    public GameObject[] playerTokens;

    private int[] currentSpace;
    private PropertyUIManager propertyUI;
    private DatabaseManager dbManager;

    void Start()
    {
        propertyUI = FindObjectOfType<PropertyUIManager>();
        dbManager = FindObjectOfType<DatabaseManager>();

        currentSpace = new int[playerTokens.Length];

        for (int i = 0; i < playerTokens.Length; i++)
        {
            int playerId = i + 1;
            currentSpace[i] = dbManager.GetPlayerPosition(playerId);
            playerTokens[i].transform.position = boardSpaces[currentSpace[i]].position;
        }
    }

    public void StartMovePlayer(int playerIndex, int diceRoll)
    {
        StartCoroutine(MovePlayer(playerIndex, diceRoll));
    }

    IEnumerator MovePlayer(int playerIndex, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            int oldPos = currentSpace[playerIndex];
            currentSpace[playerIndex] = (currentSpace[playerIndex] + 1) % boardSpaces.Length;

            if (oldPos > currentSpace[playerIndex])
            {
                int dbPlayerId = playerIndex + 1;
                float balance = dbManager.GetPlayerBalance(dbPlayerId);
                dbManager.UpdatePlayerBalance(dbPlayerId, balance + 300);
            }

            Transform nextSpace = boardSpaces[currentSpace[playerIndex]];
            Vector3 startPos = playerTokens[playerIndex].transform.position;
            Vector3 endPos = nextSpace.position;

            float elapsedTime = 0f;
            float duration = 0.5f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
                currentPos.y = Mathf.Sin(t * Mathf.PI) * 0.5f + startPos.y;
                playerTokens[playerIndex].transform.position = currentPos;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTokens[playerIndex].transform.position = endPos;
        }

        int finalPos = currentSpace[playerIndex];
        int playerId = playerIndex + 1;

        if (finalPos == 18)
        {
            currentSpace[playerIndex] = 6;
            Transform jailSpace = boardSpaces[6];
            Vector3 startPos = playerTokens[playerIndex].transform.position;
            Vector3 endPos = jailSpace.position;

            float elapsedTime = 0f;
            float fastDuration = 0.2f;

            while (elapsedTime < fastDuration)
            {
                float t = elapsedTime / fastDuration;
                playerTokens[playerIndex].transform.position = Vector3.Lerp(startPos, endPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTokens[playerIndex].transform.position = endPos;

            dbManager.SaveGame(playerId, 6, dbManager.GetPlayerBalance(playerId));
            propertyUI.wasMovedFromGoToJail = true;
            propertyUI.ShowPropertyPopup(playerId, 6);
            yield break;
        }

        float balanceFinal = dbManager.GetPlayerBalance(playerId);
        dbManager.SaveGame(playerId, currentSpace[playerIndex], balanceFinal);
        propertyUI.ShowPropertyPopup(playerId, currentSpace[playerIndex]);
    }

    public int GetPlayerPosition(int playerIndex)
    {
        return currentSpace[playerIndex];
    }
}
