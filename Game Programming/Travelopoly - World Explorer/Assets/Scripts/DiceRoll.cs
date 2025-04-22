using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DiceRoll : MonoBehaviour
{
    public Rigidbody diceRigidbody;
    public Button rollButton; 
    
    public TextMeshProUGUI player1Text; 
    public TextMeshProUGUI player2Text; 
    public PlayerTokenMovement playerTokenMovement;
    
    public GameObject propertyPopup;
    [SerializeField] private DatabaseManager databaseManager;
    

    private bool isRolling = false;
    public int currentPlayer = 1; 
    private int targetNumber; 
    public int previousPlayerPosition;
    public int currentPlayerPosition;

    

    void Start()
    {
        diceRigidbody = GetComponent<Rigidbody>();
        
        rollButton.onClick.AddListener(RollDice); 

        transform.position = new Vector3(0f, 2f, 0.16f); 

        diceRigidbody.useGravity = true;
        diceRigidbody.isKinematic = false;
        diceRigidbody.drag = 0.3f;
        diceRigidbody.angularDrag = 0.3f;

        UpdatePlayerUI(); 
        if (playerTokenMovement == null)
        {
            playerTokenMovement = FindObjectOfType<PlayerTokenMovement>();
            if (playerTokenMovement == null)
            {
                Debug.LogError("PlayerTokenMovement is NOT found in the scene!");
            }
            else
            {
                Debug.Log("PlayerTokenMovement successfully assigned.");
            }
        }
    }

    void RollDice()
    {
        if (currentPlayer == 1 && !isRolling)
        {
            isRolling = true;
            rollButton.interactable = false;

            targetNumber = Random.Range(1, 7); 
            Debug.Log("Player 1 rolled: " + targetNumber);

            StartDiceRoll();
        }
    }

  
    public void AI_RollDice()
    {
        if ( currentPlayer == 2 && !isRolling)
        {
            
            isRolling = true;
            rollButton.interactable = false; 

            targetNumber = Random.Range(1, 7); 
            Debug.Log("Player 2 rolled: " + targetNumber);

            StartDiceRoll();
        }
    }

   
    private void StartDiceRoll()
    {
        
        transform.position = new Vector3(0f, 2f, 0.16f);
        diceRigidbody.velocity = Vector3.zero;
        diceRigidbody.angularVelocity = Vector3.zero;

       
        diceRigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 5f, Random.Range(-1f, 1f)), ForceMode.Impulse);
        diceRigidbody.AddTorque(Random.Range(-300f, 300f), Random.Range(-300f, 300f), Random.Range(-300f, 300f));

        StartCoroutine(WaitForDiceToSettle()); 
    }

    
    IEnumerator WaitForDiceToSettle()
    {

        yield return new WaitForSeconds(1.5f); 

        
        diceRigidbody.velocity = Vector3.zero;
        diceRigidbody.angularVelocity = Vector3.zero;
        diceRigidbody.isKinematic = true;

        SetDiceRotation(targetNumber); 

        
        previousPlayerPosition = playerTokenMovement.GetPlayerPosition(currentPlayer - 1);
        playerTokenMovement.StartMovePlayer(currentPlayer - 1, targetNumber); 

           
        currentPlayerPosition = playerTokenMovement.GetPlayerPosition(currentPlayer - 1);
 
        diceRigidbody.isKinematic = false; 
        UpdatePlayerPositionInDatabase(targetNumber); 

        

        SwapTurn();
        isRolling = false;
        rollButton.interactable = true; 
        
    }
    

    void SetDiceRotation(int number)
    {

        Quaternion targetRotation = Quaternion.identity;
        switch (number)
        {
            case 1: targetRotation = Quaternion.Euler(-90, 0, 0); break;
            case 2: targetRotation = Quaternion.Euler(0, 0, -90); break;
            case 3: targetRotation = Quaternion.Euler(0, 0, 0); break;
            case 4: targetRotation = Quaternion.Euler(0, 0, 180); break;
            case 5: targetRotation = Quaternion.Euler(0, 0, 90); break;
            case 6: targetRotation = Quaternion.Euler(90, 0, 0); break;
        }
        transform.rotation = targetRotation;
    }

   
    void SwapTurn()
    {

        currentPlayer = (currentPlayer == 1) ? 2 : 1;

    }

   
    public void UpdatePlayerUI()
    {
        
        player1Text.color = (currentPlayer == 1) ? Color.red : Color.gray;
        player2Text.color = (currentPlayer == 2) ? Color.blue : Color.gray;
        if (currentPlayer == 2)
        {
            AI_RollDice();
        }
    
    }

    
    private void UpdatePlayerPositionInDatabase(int diceRoll)
    {

        currentPlayerPosition = playerTokenMovement.GetPlayerPosition(currentPlayer - 1);
        databaseManager.SaveGame(currentPlayer, currentPlayerPosition, databaseManager.GetPlayerBalance(currentPlayer));
    }

   
    public void SetRollButtonInteractable(bool state)
    {
      
        rollButton.interactable = state;
    }
}

