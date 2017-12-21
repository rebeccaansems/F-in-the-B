using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinUI : UI
{
    public Text TimeText, CorrectAnswerText, TotalTimeText;
    public Animator LetterParent, AnswerParent, HintButton, ClearButton;

    private bool showWin = true, resetGame = false;

    public void MakeWinVisible()
    {
        if (showWin)
        {
            showWin = false;

            OpenPopup(this.GetComponent<CanvasGroup>());
            OpenPopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Win Panel")).First());

            CorrectAnswerText.text = CurrentAnswer.s_CorrectAnswer;
            TimeSpan duration = TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));
            TimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

            PlayerPrefs.SetFloat("TotalTimeOnPuzzles", PlayerPrefs.GetFloat("TotalTimeOnPuzzles", 0) + Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));

            StartCoroutine(AddEndOfGameGem());

            PlayerStats.s_CurrentLevel++;

            if (PlayerStats.s_CurrentLevel == QuestionDatabase.s_AllQuestions.Count)
            {
                resetGame = true;

                PlayerStats.s_CurrentLevel = 0;
                PlayerStats.s_ColorHintUsed = new string('0', QuestionDatabase.s_AllQuestions.Count);
                PlayerStats.s_FillHintUsed = new string('0', QuestionDatabase.s_AllQuestions.Count);

                duration = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("TotalTimeOnPuzzles", 0));
                TotalTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

                PlayerPrefs.SetFloat("TotalTimeOnPuzzles", 0);
            }
        }
    }

    IEnumerator AddEndOfGameGem()
    {
        yield return new WaitForSeconds(1);
        PlayerStats.s_ScoreShouldUpdate = false;
        PlayerStats.s_PlayerGems += 2;
    }

    public void NextPuzzlePressed()
    {
        showWin = false;
        PlayerStats.s_ScoreShouldUpdate = true;

        if (resetGame)
        {
            resetGame = false;

            ClosePopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Win Panel")).First());
            OpenPopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Puzzles Panel")).First());
        }
        else
        {
            StartCoroutine(GoToNextLevel());

            PlayerPrefs.SetFloat("PlayerTimeOnPuzzle", 0);
            if (TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f)) > TimeSpan.FromMinutes(5) || PlayerStats.s_CurrentLevel % 3 == 0)
            {
                Advertisement.Show();
            }
            PlayerStats.s_PlayerStartPuzzleTime = Time.realtimeSinceStartup;
        }

        AnimateOuts();
        ClosePopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Win Panel")).First());
    }

    private void AnimateOuts()
    {
        LetterParent.SetBool("AnimateIn", false);
        AnswerParent.SetBool("AnimateIn", false);

        HintButton.SetBool("AnimateIn", false);
        ClearButton.SetBool("AnimateIn", false);
    }

    IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackToStart()
    {
        PlayerStats.s_CurrentLevel = 0;

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