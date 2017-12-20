using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOptions : UI
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
        OpenPopup(this.GetComponent<CanvasGroup>());
        OpenPopup(OptionsPanel);
        
        if (GemsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { GemsPanel });
        }

        if (HintsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { HintsPanel });
        }
    }

    public void CloseOptionsPanel()
    {
        ClosePopup(new CanvasGroup[] { OptionsPanel, this.GetComponent<CanvasGroup>() } );
    }
}
