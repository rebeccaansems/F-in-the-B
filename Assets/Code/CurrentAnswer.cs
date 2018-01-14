using System.Collections.Generic;
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
    public static string[] s_PlayersCorrectAnswerSeparateWords;

    public static bool s_PlayersAnswerIsNotComplete = true;

    public static ParticleSystem[][] s_EditableTileParticleSystems;
    public static List<ParticleSystem> s_BeginningTileParticleSystems;

    public void Awake()
    {
        s_CorrectAnswer = QuestionDatabase.s_AllQuestions[PlayerStats.s_CurrentLevel].Question;
        CategoryText.text = QuestionDatabase.s_AllQuestions[PlayerStats.s_CurrentLevel].Hint;
        CurrentPuzzleNumber.text = "Puzzle #" + (PlayerStats.s_CurrentLevel + 1).ToString("000");

        string editedCorrectAnswer = Regex.Replace(s_CorrectAnswer, @"[A-Z,0-9]", string.Empty);
        s_CorrectAnswerLetters = editedCorrectAnswer.Split(' ');
        s_CorrectAnswerLetters = s_CorrectAnswerLetters.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        s_PlayersCorrectAnswerSeparateWords = s_CorrectAnswerLetters;
        s_PlayersCorrectAnswer = string.Join("", s_CorrectAnswerLetters);

        s_BeginningTileParticleSystems = new List<ParticleSystem>();
        s_EditableTileParticleSystems = new ParticleSystem[s_PlayersCorrectAnswerSeparateWords.Length][];
        for (int i = 0; i < s_EditableTileParticleSystems.Length; i++)
        {
            s_EditableTileParticleSystems[i] = new ParticleSystem[s_PlayersCorrectAnswerSeparateWords[i].Length];
        }
    }

    private void Start()
    {
        s_PlayersAttempt = new string('_', s_PlayersCorrectAnswer.Length);
        s_PlayersAnswerIsNotComplete = true;
    }

    private void Update()
    {
        if (Regex.Replace(s_PlayersAttempt, "_", "") == s_PlayersCorrectAnswer)
        {
            WinUi.GetComponent<WinUI>().MakeWinVisible();
        }
    }
}
