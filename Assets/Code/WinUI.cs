using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<CanvasGroup>().interactable = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Time.timeScale = 1;
    }

    public void MakeWinVisible()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().interactable = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Time.timeScale = 0;
    }

    public void NextPuzzlePressed()
    {
        PlayerStats.s_PlayerGems += 2;
        PlayerStats.s_CurrentLevel++;

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
