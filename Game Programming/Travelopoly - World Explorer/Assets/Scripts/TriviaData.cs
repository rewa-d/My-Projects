[System.Serializable]
public class TriviaData
{
    public int Id;
    public string Question;
    public string Answer;

    public TriviaData() { }

    public TriviaData(int id, string question, string answer)
    {
        Id = id;
        Question = question;
        Answer = answer;
    }
}
