using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOptions : UI
{
    public CanvasGroup HintsPanel, GemsPanel, OptionsPanel;


    public void SuggestPuzzle()
    {
        string email = "rebecca.ansems@gmail.com";
        string subject = EscapeUrl("F in the B - Puzzle Suggestion");
        string body = EscapeUrl("Hello,\nMy puzzle suggestion is:\n");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
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

    private string EscapeUrl(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
