using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerGems;
    public static int s_CurrentLevel;

    public static string s_ColorHintUsed;
    public static string s_FillHintUsed;

    public Text PlayerGems;

    private int prevGem = 0, prevLevel = -1;

    private void Awake()
    {
        s_PlayerGems = PlayerPrefs.GetInt("PlayerGem", 100);
        s_CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        s_ColorHintUsed = PlayerPrefs.GetString("ColorHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));
        s_FillHintUsed = PlayerPrefs.GetString("FillHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));

        if (s_ColorHintUsed.Length < QuestionDatabase.s_AllQuestions.Count)
        {
            s_ColorHintUsed += new string('0', QuestionDatabase.s_AllQuestions.Count - s_ColorHintUsed.Length);
            s_FillHintUsed += new string('0', QuestionDatabase.s_AllQuestions.Count - s_FillHintUsed.Length);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
        PlayerPrefs.SetInt("CurrentLevel", s_CurrentLevel);

        PlayerPrefs.SetString("ColorHintUsed", s_ColorHintUsed);
        PlayerPrefs.SetString("FillHintUsed", s_FillHintUsed);
    }

    private void Update()
    {
        if (prevGem != s_PlayerGems)
        {
            PlayerGems.text = s_PlayerGems.ToString();
            prevGem = s_PlayerGems;
            PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
        }

        if (prevLevel != s_CurrentLevel)
        {
            prevLevel = s_CurrentLevel;
            PlayerPrefs.SetInt("CurrentLevel", s_CurrentLevel);
        }
    }
}
