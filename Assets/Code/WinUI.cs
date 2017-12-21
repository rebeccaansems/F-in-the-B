using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class WinUI : UI
{
    public Text TimeText, CorrectAnswerText;

    private bool showWin = true;

    public void MakeWinVisible()
    {
        if (showWin)
        {
            showWin = false;

            OpenPopup(this.GetComponent<CanvasGroup>());
            OpenPopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Panel")).First());

            CorrectAnswerText.text = CurrentAnswer.s_CorrectAnswer;
            TimeSpan duration = TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));
            TimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

            StartCoroutine(AddEndOfGameGem());

            PlayerStats.s_CurrentLevel++;
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