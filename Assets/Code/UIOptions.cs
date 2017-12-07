using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    public CanvasGroup HintsPanel, GemsPanel, OptionsPanel;

    public void ResetPuzzles()
    {
        PlayerPrefs.DeleteAll();

        PlayerStats.s_CurrentLevel = 0;
        PlayerStats.s_PlayerStartPuzzleTime = 0;

        PlayerStats.s_ColorHintUsed = new string('0', QuestionDatabase.s_AllQuestions.Count);
        PlayerStats.s_FillHintUsed = new string('0', QuestionDatabase.s_AllQuestions.Count);

        Application.Quit();
    }

    public void MuteSfx()
    {

    }

    public void MuteMusic()
    {

    }

    public void OpenOptionsPanel()
    {
        UIPopup.OpenPopup(this.GetComponent<CanvasGroup>());
        UIPopup.OpenPopup(OptionsPanel);

        UIPopup.ClosePopup(HintsPanel);
        UIPopup.ClosePopup(GemsPanel);
    }

    public void CloseOptionsPanel()
    {
        UIPopup.ClosePopup(OptionsPanel);
        UIPopup.ClosePopup(this.GetComponent<CanvasGroup>());
    }
}
