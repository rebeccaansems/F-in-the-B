using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAnswer : MonoBehaviour
{
    public WinUI WinUi;
    public Text CategoryText, CurrentPuzzleNumber;

    public static string s_CorrectAnswer;
    public static string s_PlayersCorrectAnswer;
    public static string s_PlayersAttempt = "";

    public static string[] s_CorrectAnswerLetters;

    public static bool s_PlayersAnswerIsNotComplete = true;

    public void Awake()
    {
        s_CorrectAnswer = QuestionDatabase.s_AllQuestions[PlayerStats.s_CurrentLevel].Question;
        CategoryText.text = QuestionDatabase.s_AllQuestions[PlayerStats.s_CurrentLevel].Hint;
        CurrentPuzzleNumber.text = "Puzzle #" + PlayerStats.s_CurrentLevel.ToString("000");

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
