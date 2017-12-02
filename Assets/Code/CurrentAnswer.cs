using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CurrentAnswer : MonoBehaviour
{
    public static string s_CorrectAnswer = "5 Vowels IN THE Alphabet";
    public static string s_PlayersCorrectAnswer = "5 Vowels IN THE Alphabet";
    public static string[] s_CorrectAnswerLetters;

    public static string s_PlayersAttempt = "";
    public static bool s_PlayersAnswerIsNotComplete = true;

    public void Awake()
    {
        string editedCorrectAnswer = Regex.Replace(s_CorrectAnswer, @"[A-Z,0-9]", string.Empty);
        s_CorrectAnswerLetters = editedCorrectAnswer.Split(' ');
        s_CorrectAnswerLetters = s_CorrectAnswerLetters.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        s_PlayersCorrectAnswer = string.Join("", s_CorrectAnswerLetters);
    }

    private void Update()
    {
        if(s_PlayersAttempt == s_PlayersCorrectAnswer)
        {
            Debug.Log("WINNER");
        }
    }
}
