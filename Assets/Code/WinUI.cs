using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public Text TimeText, CorrectAnswerText;

    public CanvasGroup PopupUi;

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
            TimeSpan duration = TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));
            TimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

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
            PlayerPrefs.SetFloat("PlayerTimeOnPuzzle", 0);

            if (TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f)) > TimeSpan.FromMinutes(5) || PlayerStats.s_CurrentLevel % 3 == 0)
            {
                Advertisement.Show();
            }

            PlayerStats.s_PlayerStartPuzzleTime = Time.realtimeSinceStartup;
        }
    }
}