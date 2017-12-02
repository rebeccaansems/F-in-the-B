using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CurrentAnswer : MonoBehaviour
{
    public WinUI WinUi;

    public static string s_CorrectAnswer;
    public static string s_PlayersCorrectAnswer;
    public static string s_PlayersAttempt = "";

    public static string[] s_CorrectAnswerLetters;

    public static bool s_PlayersAnswerIsNotComplete = true;

    private static int currentLevel = 0;

    public void Awake()
    {
        s_CorrectAnswer = QuestionDatabase.s_AllQuestions[currentLevel].Question;
        currentLevel++;

        string editedCorrectAnswer = Regex.Replace(s_CorrectAnswer, @"[A-Z,0-9]", string.Empty);
        s_CorrectAnswerLetters = editedCorrectAnswer.Split(' ');
        s_CorrectAnswerLetters = s_CorrectAnswerLetters.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        s_PlayersCorrectAnswer = string.Join("", s_CorrectAnswerLetters);
    }

    private void Update()
    {
        if (s_PlayersAttempt == s_PlayersCorrectAnswer)
        {
            WinUi.GetComponent<WinUI>().MakeWinVisible();
            ResetComponents();
        }
    }

    private void ResetComponents()
    {
        s_PlayersAttempt = "";
        s_PlayersAnswerIsNotComplete = true;
    } 
}
