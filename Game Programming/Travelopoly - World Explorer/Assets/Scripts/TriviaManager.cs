using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class TriviaManager : MonoBehaviour
{
    
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public TMP_Text resultText;
    public Button submitButton;
    private PropertyUIManager propertyUIManager;
    public TMP_Text submitButtonText;

    private string correctAnswer;
    private int questionId;
    private int currentPlayerId;
    private DatabaseManager dbManager;

    
    

    void Start()
    {
        dbManager = FindObjectOfType<DatabaseManager>();

        propertyUIManager = FindObjectOfType<PropertyUIManager>();
        

        resultText.text = "";
    }
    
    public void SetPropertyUIManager(PropertyUIManager manager)
    {
        propertyUIManager = manager;
    }
    
    [System.Serializable]
    public class AnswerData
    {
        public int question_id;
        public bool is_correct;
    }

    IEnumerator SubmitData(int questionId, bool isCorrect)
    {
        string url = "http://127.0.0.1:5000/submit_answer";

        
        var jsonData = JsonUtility.ToJson(new AnswerData
        {
            question_id = questionId,
            is_correct = isCorrect
        });

        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log("Answer submitted successfully");
        }
    }

  
    public void FetchTriviaFromAPI(int playerId)
    {
        currentPlayerId = playerId;
        StartCoroutine(GetTriviaFromAPI());
    }

    IEnumerator GetTriviaFromAPI()
    {
        string url = "http://127.0.0.1:5000/get_trivia"; // The Flask server URL
        UnityWebRequest request = new UnityWebRequest(url, "GET");

        try
        {
            request.downloadHandler = new DownloadHandlerBuffer();

           
        }
        catch(System.Exception e)
        {
            Debug.Log($"{e}");
        }
        yield return request.SendWebRequest();

        try
        {
            Debug.Log("Fetching trivia...");
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Trivia fetched successfully!");

                string responseText = request.downloadHandler.text;
                
                TriviaResponse triviaResponse = JsonUtility.FromJson<TriviaResponse>(responseText);
                Debug.Log($"{triviaResponse}");
                Debug.Log($"{triviaResponse.question}");
                questionText.text = triviaResponse.question;
                correctAnswer = triviaResponse.correct_answer;
                questionId = triviaResponse.questionId;
                SetButtonText(submitButtonText, "Submit");

                submitButton.onClick.RemoveAllListeners();
                submitButton.onClick.AddListener(SubmitAnswer);

                FindObjectOfType<DiceRoll>().SetRollButtonInteractable(false);
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
            }
        }

        catch(System.Exception e)
        {
            Debug.Log($"{e}");
        }

        
    }

    void SubmitAnswer()
    {
        int attempts = dbManager.GetAttemptCount(currentPlayerId, questionId);

        if (attempts >= 3)
        {
            resultText.text = "No more attempts for this question!";
            StartCoroutine(ClosePanelAfterDelay());
            return;
        }

        string userAnswer = answerInput.text.Trim();

        if (string.IsNullOrEmpty(userAnswer))
        {
            resultText.text = "Please enter an answer!";
            return;
        }

        bool isCorrect = userAnswer.ToLower() == correctAnswer.ToLower();
        int newAttempts = attempts + 1;

        dbManager.RecordTriviaAttempt(currentPlayerId, questionId, isCorrect);

        if (isCorrect)
        {
            resultText.text = "Correct! Earned Rs. 100!";
            dbManager.UpdatePlayerBalance(currentPlayerId, dbManager.GetPlayerBalance(currentPlayerId) + 100);
            StartCoroutine(SubmitData(questionId, true));
            StartCoroutine(ClosePanelAfterDelay());
        }
        else
        {
            if (newAttempts >= 3)
            {
                resultText.text = "No more attempts for this question!";
                StartCoroutine(SubmitData(questionId, false));
                StartCoroutine(ClosePanelAfterDelay());
            }
            else
            {
                resultText.text = $"Wrong! Attempts left: {3 - newAttempts}. Try again:";
                answerInput.text = "";  // Clear input field for the next attempt
                submitButton.onClick.RemoveAllListeners();
                submitButton.onClick.AddListener(SubmitAnswer);  // Reassign the listener to retry submission
            }
        }

        dbManager.UpdatePlayerAttempts(currentPlayerId, questionId, newAttempts);
    }


    IEnumerator checkAnswer(string userAnswer)
    {
        string url = "http://127.0.0.1:5000/check_answer";
        string jsonData = "{\"answer\":\"" + userAnswer + "\",\"correct_answer\":\"" + correctAnswer + "\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            
            string responseText = request.downloadHandler.text;
            AnswerResponse answerResponse = JsonUtility.FromJson<AnswerResponse>(responseText);

            correctAnswer = answerResponse.result;
        }
        else
        {
            Debug.LogError("API call failed: " + request.error);
        }
    }
    
    IEnumerator ClosePanelAfterDelay()
    {
        Debug.Log($"Inside close panel");
        yield return new WaitForSeconds(2f);
        

        propertyUIManager.UpdatePlayerUI();
        propertyUIManager.triviaCanvas.SetActive(false);
        propertyUIManager.propertyPopup.SetActive(false);
        FindObjectOfType<DiceRoll>().SetRollButtonInteractable(true);
        FindObjectOfType<DiceRoll>().UpdatePlayerUI();

        resultText.text = "";
        questionText.text = "";
        answerInput.text = "";



    }

    void SetButtonText(TMP_Text text, string value)
    {
        if (text != null) text.text = value;
    }

    [System.Serializable]
    public class TriviaResponse
    {
        public string question;
        public string correct_answer;
        public int questionId;
        public int complexity;
    }

    [System.Serializable]
    public class AnswerResponse
    {
        public string result;
        public int reward;
    }

    
}
