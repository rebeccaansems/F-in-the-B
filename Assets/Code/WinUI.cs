﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public Text TimeText, CorrectAnswerText;

    private bool showWin = true;

    void Start()
    {
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<CanvasGroup>().interactable = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Time.timeScale = 1;
    }

    public void MakeWinVisible()
    {
        if (showWin)
        {
            showWin = false;

            this.GetComponent<CanvasGroup>().alpha = 1;
            this.GetComponent<CanvasGroup>().interactable = true;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;

            CorrectAnswerText.text = CurrentAnswer.s_CorrectAnswer;
            TimeText.text = TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle")).ToString();

            PlayerStats.s_PlayerStartPuzzleTime = Time.realtimeSinceStartup;
            PlayerStats.s_ScoreShouldUpdate = false;
            PlayerStats.s_PlayerGems += 2;

            PlayerStats.s_CurrentLevel++;
        }
    }

    public void NextPuzzlePressed()
    {
        showWin = false;
        PlayerStats.s_ScoreShouldUpdate = true;

        if (PlayerStats.s_CurrentLevel < QuestionDatabase.s_AllQuestions.Count)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (PlayerStats.s_CurrentLevel % 3 == 0)
            {
                Advertisement.Show();
            }
        }
    }
}
