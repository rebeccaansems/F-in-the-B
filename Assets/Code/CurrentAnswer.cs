using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAnswer : MonoBehaviour
{
    public WinUI WinUi;
    public Text CategoryText;
    
    public static string s_CorrectAnswer;
    public static string s_PlayersCorrectAnswer;
    public static string s_PlayersAttempt = "";

    public static string[] s_CorrectAnswerLetters;

    public static bool s_PlayersAnswerIsNotComplete = true;

    private static int currentLevel = 0;

    public void Awake()
    {
        s_CorrectAnswer = QuestionDatabase.s_AllQuestions[currentLevel].Question;
        CategoryText.text = QuestionDatabase.s_AllQuestions[currentLevel].Category;
        currentLevel++;

        string editedCorrectAnswer = Regex.Replace(s_CorrectAnswer, @"[A-Z,0-9]", string.Empty);
        s_CorrectAnswerLetters = editedCorrectAnswer.Split(' ');
        s_CorrectAnswerLetters = s_CorrectAnswerLetters.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        s_PlayersCorrectAnswer = string.Join("", s_CorrectAnswerLetters);
    }

    private void Start()
    {
        s_PlayersAttempt = "";
        s_PlayersAnswerIsNotComplete = true;
    }

    private void Update()
    {
        if (s_PlayersAttempt == s_PlayersCorrectAnswer)
        {
            WinUi.GetComponent<WinUI>().MakeWinVisible();
        }
    }
}
