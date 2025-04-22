using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropertyUIManager : MonoBehaviour
{
    public DiceRoll diceRoll;
    public GameObject propertyPopup;
    public Image propertyImage;
    public TMP_Text actionText;
    public GameObject triviaCanvas;
    public PlayerTokenMovement playerTokenMovement;
    private bool triviaFlag = false;
    public TMP_Text msg;

    public Button buyButton, buyAttractionButton, mortgageButton, skipButton, payRentButton, continueButton;
    public TMP_Text buyButtonText, buyAttractionButtonText, mortgageButtonText, skipButtonText, payRentButtonText, continueButtonText;
    public TMP_Text player1BalText, player2BalText;
    public TMP_Text player1NameText, player2NameText;

    private static PropertyUIManager instance;
    private int currentPropertyId;
    private int currentPlayerId;
    private DatabaseManager dbManager;
    private int pos;
    
    public bool wasMovedFromGoToJail = false;

    public TriviaManager triviaManager;  

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
        if (diceRoll == null)
        {
            Debug.LogError("DiceRoll script not found!");
        }
        dbManager = FindObjectOfType<DatabaseManager>();

        buyButton.onClick.RemoveAllListeners();
        buyAttractionButton.onClick.RemoveAllListeners();
        mortgageButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();
        payRentButton.onClick.RemoveAllListeners();

        buyButton.onClick.AddListener(BuyProperty);
        buyAttractionButton.onClick.AddListener(BuyAttraction);
        mortgageButton.onClick.AddListener(ToggleMortgage);
        skipButton.onClick.AddListener(ClosePopup);
        payRentButton.onClick.AddListener(PayRent);

        triviaCanvas.SetActive(false);

        triviaManager = FindObjectOfType<TriviaManager>();
        UpdatePlayerNames();
        UpdatePlayerUI();
        propertyPopup.SetActive(false);
    }
    void UpdatePlayerNames()
    {
        // Retrieve the player names from PlayerPrefs
        string player1Name = PlayerPrefs.GetString("Player1Name", "Player 1");
        string player2Name = PlayerPrefs.GetString("Player2Name", "AI");

        player1NameText.text = player1Name;
        player2NameText.text = player2Name;

        dbManager.UpdatePlayerNames(player1Name, player2Name);
        
        player1BalText.text = $"Balance:\nRs.{dbManager.GetPlayerBalance(1)}";
        player2BalText.text = $"Balance:\nRs.{dbManager.GetPlayerBalance(2)}";
    }
    public void ShowPropertyPopup(int playerId, int position)
    {
        actionText.alignment = TextAlignmentOptions.Center;
        actionText.fontSize = 50;
        actionText.lineSpacing = 15;

        currentPlayerId = playerId;
        //Debug.Log($"Previous position {diceRoll.previousPlayerPosition}");
        //Debug.Log($"Current position {position}");

        
        if (position == 0)
        {
            HandleGoSpace(playerId);
            return;
        }

       
        if (position > 0 && position < diceRoll.previousPlayerPosition)
        {
            
            HandlePassGo(playerId);
           
        }
        if (position == 6 && wasMovedFromGoToJail)
        {
            wasMovedFromGoToJail = false;
            MovePlayerToJail(playerId);
            return;
        }

        if (position == 6)
        {
            actionText.text = "You are visiting Jail. Nothing happens.";
            DisableAllButtons();
            SetButtonText(continueButtonText, "Continue");
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ClosePopup);
            continueButton.gameObject.SetActive(true);
            propertyPopup.SetActive(true);
            PopupPropertySpriteShow(playerId, position);
            return;
        }
        if (position == 2 || position == 16)
        {
            actionText.text = "Work in progress.." ;
            DisableAllButtons();
            SetButtonText(continueButtonText, "Continue");
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ClosePopup);
            continueButton.gameObject.SetActive(true);
            propertyPopup.SetActive(true);
            PopupPropertySpriteShow(playerId, position);
            return;
        }

        if (position == 12)
        {
            actionText.text = "You landed on a Rest Stop. Take a break!";
            DisableAllButtons();
            SetButtonText(continueButtonText, "Continue");
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ClosePopup);
            continueButton.gameObject.SetActive(true);
            propertyPopup.SetActive(true);
            PopupPropertySpriteShow(playerId, position);
            return;
        }

        if (position == 18)
        {
            MovePlayerToJail(playerId);
        }

        if (position == 8 || position == 22)
        {
            actionText.text = "You landed on a Travel Card!\nAnswer to earn rewards!";
            DisableAllButtons();
            SetButtonText(continueButtonText, "Continue");
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                triviaFlag = true;
                ClosePopup();
                //Debug.Log("Trivia flag"+ triviaFlag);
                triviaCanvas.SetActive(true);
                triviaManager.FetchTriviaFromAPI(currentPlayerId);
               

            });
            continueButton.gameObject.SetActive(true);
            propertyPopup.SetActive(true);
            PopupPropertySpriteShow(playerId, position);
            return;
        }


        var propertyData = dbManager.GetPropertyDetailsByPosition(position);
        var ownerId = dbManager.GetPropertyOwner(propertyData.Id);
       
        currentPropertyId = propertyData.Id;

        PopupPropertySpriteShow(playerId, position);

        if (ownerId == 0)
        {
            DisableAllButtons();
            actionText.text = "Do you want to buy this property?";
            SetButtonText(buyButtonText, "Buy Property");
            SetButtonText(skipButtonText, "Skip");
            buyButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
        }
        else if (ownerId == playerId)
        {
            DisableAllButtons();
            var attractionLevel = dbManager.GetPropertyAttractionLevel(propertyData.Id, ownerId);
            
            if (attractionLevel == 2)
            {
                actionText.text = "Mortgage!";
            }
            else
            {
                actionText.text = "Upgrade or mortgage.";
                SetButtonText(buyAttractionButtonText, "Upgrade Property");
                buyAttractionButton.gameObject.SetActive(true);
            }
            SetButtonText(mortgageButtonText, "Mortgage Property");
            SetButtonText(skipButtonText, "Skip");
            
            mortgageButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
        }
        else
        {
            float rentAmount = dbManager.GetUpdatedRent(currentPropertyId);
            actionText.text = $"Pay Rent: Rs.{rentAmount}";
            DisableAllButtons();
            SetButtonText(payRentButtonText, $"Pay Rs. {rentAmount}");
            payRentButton.gameObject.SetActive(true);
        }


        propertyPopup.SetActive(true);
    }

    public void PopupPropertySpriteShow(int playerId, int position)
    {
        
        var propertySprite = Resources.Load<Sprite>($"PropertyCards/position_{position}");
        if (propertySprite != null)
        {
            
            propertyImage.sprite = propertySprite;
            propertyImage.gameObject.SetActive(true);
        }
        else
        {
            propertyImage.gameObject.SetActive(false);
        }
        

        if (playerId == 2)
        {
 
            SimulateAIActions(2, position);
        }
    }

    public void BuyProperty()
    {
        Debug.Log("The current playerId in Buy Property " + currentPlayerId);
        float balance = dbManager.GetPlayerBalance(currentPlayerId);
        float cost = dbManager.GetPropertyCost(currentPropertyId);
        if (balance >= cost)
        {
            
            dbManager.UpdatePlayerBalance(currentPlayerId, balance - cost);
            dbManager.LogPropertyPurchase(currentPropertyId, currentPlayerId);
            UpdatePlayerUI();
        }
        ClosePopup();
    }

    public void BuyAttraction()
    {
        float cost = dbManager.GetAttractionCost(currentPropertyId);
        float balance = dbManager.GetPlayerBalance(currentPlayerId);
        if (balance >= cost)
        {
            dbManager.UpdatePlayerBalance(currentPlayerId, balance - cost);
            dbManager.LogAttractionUpgrade(currentPropertyId, currentPlayerId);
            UpdatePlayerUI();
        }
        ClosePopup();
    }

    public void PayRent()
    {
        Debug.Log("In Pay rent");
        Debug.Log(currentPlayerId);
        int ownerId = dbManager.GetPropertyOwner(currentPropertyId);
        float rent = dbManager.GetUpdatedRent(currentPropertyId);
        float balance = dbManager.GetPlayerBalance(currentPlayerId);
        Debug.Log(rent);
        Debug.Log(balance);
        if (balance >= rent)
        {
            dbManager.UpdatePlayerBalance(currentPlayerId, balance - rent);
            dbManager.UpdatePlayerBalance(ownerId, dbManager.GetPlayerBalance(ownerId) + rent);
            dbManager.LogRentPayment(currentPropertyId, ownerId);
            UpdatePlayerUI();
        }
            ClosePopup();
    }

    void ToggleMortgage()
    {
        dbManager.LogMortgage(currentPropertyId, currentPlayerId);
        ClosePopup();
    }

    void MovePlayerToJail(int playerId)
    {
        int jailPosition = 6; 
        Debug.Log($"Moving Player {playerId} to Jail!");

        dbManager.SaveGame(playerId, jailPosition, dbManager.GetPlayerBalance(playerId));

        actionText.text = "You are in Jail! Pay Rs. 100 to leave.";
        DisableAllButtons();
        SetButtonText(continueButtonText, "Pay Rs. 100");
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => PayJailFee());  


        continueButton.gameObject.SetActive(true);

        Sprite jailSprite = Resources.Load<Sprite>("PropertyCards/position_6");
        if (jailSprite != null)
        {
            propertyImage.sprite = jailSprite;
            propertyImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Jail sprite not found!");
            propertyImage.gameObject.SetActive(false);
        }

        propertyPopup.SetActive(true);
    }


    void PayJailFee()
    {
        float playerBalance = dbManager.GetPlayerBalance(currentPlayerId);

        if (playerBalance >= 100)
        {
            float newBalance = playerBalance - 100;
            dbManager.UpdatePlayerBalance(currentPlayerId, newBalance);  
            Debug.Log($"Player {currentPlayerId} paid Rs. 100 and is free!");

            UpdatePlayerUI();  

            actionText.text = "You are free! Move on.";
            DisableAllButtons();
            SetButtonText(continueButtonText, "Continue");

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ClosePopup);
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            actionText.text = "Not enough balance to pay jail fee!";
            Debug.Log($"Player {currentPlayerId} does not have enough balance to leave jail.");
        }
    }

    void SetButtonText(TMP_Text text, string value)
    {
        if (text != null) text.text = value;
    }

    public void UpdatePlayerUI()
    {
        float p1 = dbManager.GetPlayerBalance(1);
        float p2 = dbManager.GetPlayerBalance(2);
        player1BalText.text = $"Balance:\nRs.{p1}";
        player2BalText.text = $"Balance:\nRs.{p2}";
    }

    public void ClosePopup()
    {
       
        propertyPopup.SetActive(false);
        if (triviaFlag == true)
        {
            
            triviaFlag = false;

        }
        else
        {

            diceRoll.UpdatePlayerUI();
        }
               

    }

    public void OpenTrivia()
    {
        
        triviaCanvas.SetActive(true);
        
    }
    void HandleGoSpace(int playerId)
    {
       
        actionText.text = "You landed on GO! Collect Rs. 300!";
        DisableAllButtons();
        SetButtonText(continueButtonText, "Continue");
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ClosePopup);
        continueButton.gameObject.SetActive(true);
        propertyPopup.SetActive(true);
                                                    
        UpdatePlayerUI();  
        PopupPropertySpriteShow(playerId, 0); 
    }

    void HandlePassGo(int playerId)
    {
        
        actionText.text = "You passed GO! Collect Rs. 300!";
        DisableAllButtons();
        SetButtonText(continueButtonText, "Continue");
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ClosePopup);
        continueButton.gameObject.SetActive(true);
        propertyPopup.SetActive(true);
        UpdatePlayerUI();
        PopupPropertySpriteShow(playerId, 0); 

        
    }

    public void SimulateAIActions(int playerId, int position)
    {
        Debug.Log("IN SIMULATE");
        string message ="";
        if (position == 6 && wasMovedFromGoToJail)
        {
            wasMovedFromGoToJail = false;
            MovePlayerToJail(playerId);
            message = "AI clicked on the Continue button.";
        }
        if (position == 8 || position == 22)
        {
            triviaFlag = false;
            message = "AI received Rs. 100 for answering correctly the triva question!!";
            dbManager.UpdatePlayerBalance(playerId, dbManager.GetPlayerBalance(playerId) + 100);
            UpdatePlayerUI();
        }
            if (position == 2 || position == 16 || position == 6 || position == 12 || position == 0)
        {
            //continueButton.onClick.Invoke();
            message = "AI clicked on the Continue button.";
        }
        if (position > 0 && position < diceRoll.previousPlayerPosition)
        {
            continueButton.onClick.Invoke();
            message = "AI clicked on the Continue button.";
        }
        if (position == 18)
        {
            MovePlayerToJail(playerId);
            continueButton.onClick.Invoke();
            message = "AI moved to the Jail and paid Rs. 100 to be released.";
        }
        else if (position != 8 && position != 22 && position != 0 && position != 2 && position != 18 && position != 6 && position != 12 && position != 16) 
        {
            
            var propertyData = dbManager.GetPropertyDetailsByPosition(position);
            
            var ownerId = dbManager.GetPropertyOwner(propertyData.Id);
            
            
            if (ownerId == 0)
            {
               
                if (dbManager.GetPlayerBalance(playerId) >= propertyData.Cost)
                {
                    
                    buyButton.onClick.Invoke();
                    message = "AI bought a property.";
                }
                else
                {
                    
                    skipButton.onClick.Invoke();
                    message = "AI skipped buying a property due to lack of funds.";
                }
            }
            else if (ownerId != playerId)
            {
                
                
                payRentButton.onClick.Invoke();
                message = "AI paid rent to the other player.";
            }
            else
            {
                
                message = HandleOwnedProperty(playerId, propertyData.Id);
            }

        }
        
        
        Invoke(nameof(ClosePopup), 1.5f);

        NotifyOtherPlayer(message);
    }

    public string  HandleOwnedProperty(int playerId, int propertyId)
    {
        string message;
        var attractionLevel = dbManager.GetPropertyAttractionLevel(propertyId, playerId);
        if (attractionLevel < 2) 
        {
           
            buyAttractionButton.onClick.Invoke();
            message = "AI purchase an Attraction to earn more money from rent.";
        }
        else
        {
   
            skipButton.onClick.Invoke();
            message = "AI did not upgrade. AI clicked on the Skip button";
        }
        return message;
    }

    private void SkipAction()
    {
        
        ClosePopup();  
    }

    private void NotifyOtherPlayer(string message)
    {
       
        if (currentPlayerId == 2)  
        {
            msg.text = message;
            
            
        }
    }

    
    
    void DisableAllButtons()
    {
        buyButton.gameObject.SetActive(false);
        buyAttractionButton.gameObject.SetActive(false);
        mortgageButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        payRentButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }
}
