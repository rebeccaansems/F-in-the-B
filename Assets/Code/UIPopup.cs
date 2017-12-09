using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UIPopup : UI
{

    private const int k_Skip = 50, k_Reveal = 20, k_Color = 10;

    public FillAnswerPanel FillAnswer;
    public FillLetterPanel FillLetter;

    public Button RevealButton, ColorButton;

    public CanvasGroup HintsPanel, GemsPanel, OptionsPanel;
    public WinUI WinUi;

    void Start()
    {
        ClosePopup(this.GetComponent<CanvasGroup>());
        ClosePopup(HintsPanel);
        ClosePopup(GemsPanel);
        ClosePopup(OptionsPanel);

        if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '0')
        {
            ColorButton.interactable = true;
            ColorButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = false;
        }
        else if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            ColorButton.interactable = false;
            ColorButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;
        }

        if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '0')
        {
            RevealButton.interactable = true;
            RevealButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = false;
        }
        else if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            RevealButton.interactable = false;
            RevealButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;
        }
    }
   

    public void OpenHintsPanel()
    {
        OpenPopup(this.GetComponent<CanvasGroup>());
        OpenPopup(HintsPanel);
    }

    public void CloseHintPanel()
    {
        ClosePopup(this.GetComponent<CanvasGroup>());
        ClosePopup(HintsPanel);
    }

    public void OpenGemsPanel()
    {
        OpenPopup(this.GetComponent<CanvasGroup>());
        OpenPopup(GemsPanel);
    }

    public void CloseGemsPanel()
    {
        ClosePopup(GemsPanel);

        if(HintsPanel.alpha == 0)
        {
            ClosePopup(this.GetComponent<CanvasGroup>());
        }
    }


    public void UseColorHint()
    {
        if (PlayerStats.s_PlayerGems < k_Color)
        {
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Color;
            PlayerStats.s_ColorHintUsed = PlayerStats.s_ColorHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            ColorButton.interactable = false;
            ColorButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;

            FillLetter.ColorRequiredTiles();
            FillLetter.DisableNotRequiredTiles();

            ClosePopup(HintsPanel);
            ClosePopup(this.GetComponent<CanvasGroup>());
        }
    }

    public void UseRevealHint()
    {
        if (PlayerStats.s_PlayerGems < k_Reveal)
        {
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Reveal;
            PlayerStats.s_FillHintUsed = PlayerStats.s_FillHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            RevealButton.interactable = false;
            RevealButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;

            FillAnswer.FillFirstWord();

            ClosePopup(HintsPanel);
            ClosePopup(this.GetComponent<CanvasGroup>());
        }
    }

    public void UseSkipHint()
    {
        if (PlayerStats.s_PlayerGems < k_Skip)
        {
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Skip;

            WinUi.MakeWinVisible();

            ClosePopup(HintsPanel);
            ClosePopup(this.GetComponent<CanvasGroup>());
        }
    }

    public void UseWatchAd()
    {
        if (!Advertisement.IsReady("rewardedVideo"))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", "rewardedVideo"));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show("rewardedVideo", options);
    }



    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                PlayerStats.s_PlayerGems += 10;
                break;
            case ShowResult.Skipped:
                PlayerStats.s_PlayerGems += 10;
                break;
            case ShowResult.Failed:
                break;
        }
    }
}
