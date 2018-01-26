using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;
using VoxelBusters.NativePlugins;

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

            if (PlayerStats.s_TutorialOn == 1)
            {
                PlayerStats.Instance.TutorialFinished();
            }

            if (this != null)
            {
                this.GetComponent<PlayAudio>().PlayRandom();

                OpenPopup(this.GetComponent<CanvasGroup>());
                OpenPopup(this.GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Win Panel")).First());

                CorrectAnswerText.text = CurrentAnswer.s_CorrectAnswer;
                TimeSpan duration = TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                    + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));
                TimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

                PlayerPrefs.SetFloat("TotalTimeOnPuzzles", PlayerPrefs.GetFloat("TotalTimeOnPuzzles", 0) + Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                    + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));

                PlayerStats.Instance.ChangeGems(2);
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
    }

    public void NextPuzzlePressed()
    {
        showWin = false;

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

            if (PlayerStats.Instance.ShowAds)
            {
                if (TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f)) > TimeSpan.FromMinutes(PlayerStats.k_MinuesUntilAd)
                || PlayerStats.s_CurrentLevel % PlayerStats.k_LevelsUntilAd == 0)
                {
                    GameObject.FindGameObjectsWithTag("Background").Select(x => x.GetComponent<BackgroundSelection>()).ToList().First().ChangeBackground();
                    Advertisement.Show();
                }
            }
            PlayerStats.s_CurrentLevel += 1;
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

        PlayerPrefs.SetString("ColorHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));
        PlayerPrefs.SetString("FillHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));

        PlayerStats.s_ColorHintUsed = PlayerPrefs.GetString("ColorHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));
        PlayerStats.s_FillHintUsed = PlayerPrefs.GetString("FillHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("PlayerTimeOnPuzzle", 0);

        if (PlayerStats.Instance.ShowAds)
        {
            if (TimeSpan.FromSeconds(Time.realtimeSinceStartup - PlayerStats.s_PlayerStartPuzzleTime
                + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f)) > TimeSpan.FromMinutes(PlayerStats.k_MinuesUntilAd)
                || PlayerStats.s_CurrentLevel % PlayerStats.k_LevelsUntilAd == 0)
            {
                var backgrounds = GameObject.FindGameObjectsWithTag("Background").Select(x => x.GetComponent<BackgroundSelection>()).ToList();
                backgrounds.ForEach(x => x.ChangeBackground());
                Advertisement.Show();
            }
        }

        PlayerStats.s_PlayerStartPuzzleTime = Time.realtimeSinceStartup;
    }

    public void Share()
    {
        SocialShareSheet _shareSheet = new SocialShareSheet();
        _shareSheet.Text = "I've just finished puzzle #" + (PlayerStats.s_CurrentLevel + 1) + " on F in the B, can you beat my time?";

#if UNITY_IOS
        _shareSheet.URL = "https://itunes.apple.com/us/app/f-in-the-b/id1328718409?ls=1&mt=8";
#elif UNITY_ANDROID
        _shareSheet.URL = m_shareURL;
#endif

        _shareSheet.AttachScreenShot();

        // Show composer
        NPBinding.UI.SetPopoverPointAtLastTouchPosition(); // To show popover at last touch point on iOS. On Android, its ignored.
        NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
    }



    private void FinishedSharing(eShareResult _result)
    {
        if (_result == eShareResult.CLOSED)
        {
            PlayerStats.Instance.ChangeGems(5);
        }
    }
}