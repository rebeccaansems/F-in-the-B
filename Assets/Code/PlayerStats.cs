﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerGems;
    public static int s_CurrentLevel;

    public static string s_ColorHintUsed;
    public static string s_FillHintUsed;

    public static float s_PlayerStartPuzzleTime;

    public static bool s_ScoreShouldUpdate;

    public Text PlayerGems;

    public ParticleSystem LoseGemsParticleSystem, GainGemsParticleSystem;

    private int prevGem = -1, prevLevel = -1;

    private void Awake()
    {
        s_ScoreShouldUpdate = true;

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

        PlayerPrefs.SetFloat("PlayerTimeOnPuzzle", Time.realtimeSinceStartup - s_PlayerStartPuzzleTime + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));

        PlayerPrefs.SetString("ColorHintUsed", s_ColorHintUsed);
        PlayerPrefs.SetString("FillHintUsed", s_FillHintUsed);
    }

    private void Update()
    {
        if (prevGem != s_PlayerGems && s_ScoreShouldUpdate)
        {
            if (prevGem > s_PlayerGems)
            {
                LoseGemsParticleSystem.Play();
            }
            else
            {
                GainGemsParticleSystem.Play();
            }

            prevGem = s_PlayerGems;
            PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
            PlayerGems.text = s_PlayerGems.ToString();
        }

        if (prevLevel != s_CurrentLevel)
        {
            prevLevel = s_CurrentLevel;
            PlayerPrefs.SetInt("CurrentLevel", s_CurrentLevel);
        }
    }
}
