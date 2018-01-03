using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOptions : UI
{
    public CanvasGroup HintsPanel, GemsPanel, OptionsPanel;
    public Image MusicImage, SFXImage;
    public Sprite AudioOn, AudioOff;

    public void Start()
    {
        if (PlayerStats.s_SFXAudio == 0)
        {
            SFXImage.sprite = AudioOff;
        }
        else
        {
            SFXImage.sprite = AudioOn;
        }

        if (PlayerStats.s_MusicAudio == 0)
        {
            MusicImage.sprite = AudioOff;
        }
        else
        {
            MusicImage.sprite = AudioOn;
        }
    }

    public void SuggestPuzzle()
    {
        string email = "rebecca.ansems@gmail.com";
        string subject = EscapeUrl("F in the B - Puzzle Suggestion");
        string body = EscapeUrl("Hello,\nMy puzzle suggestion is:\n");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void MuteSfx()
    {
        if (PlayerStats.s_SFXAudio == 1)
        {
            PlayerStats.s_SFXAudio = 0;
            PlayerPrefs.SetInt("SFXAudio", PlayerStats.s_SFXAudio);
            SFXImage.sprite = AudioOff;
        }
        else
        {
            PlayerStats.s_SFXAudio = 1;
            PlayerPrefs.SetInt("SFXAudio", PlayerStats.s_SFXAudio);
            SFXImage.sprite = AudioOn;
        }
    }

    public void MuteMusic()
    {
        if (PlayerStats.s_MusicAudio == 1)
        {
            PlayerStats.s_MusicAudio = 0;
            PlayerPrefs.SetInt("MusicAudio", PlayerStats.s_MusicAudio);
            MusicImage.sprite = AudioOff;
        }
        else
        {
            PlayerStats.s_MusicAudio = 1;
            PlayerPrefs.SetInt("MusicAudio", PlayerStats.s_MusicAudio);
            MusicImage.sprite = AudioOn;
        }
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
        ClosePopup(new CanvasGroup[] { OptionsPanel, this.GetComponent<CanvasGroup>() });
    }

    private string EscapeUrl(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
