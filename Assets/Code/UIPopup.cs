using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

public class UIPopup : UI
{
    private const int k_Skip = 50, k_Reveal = 20, k_Color = 10;

    public FillAnswerPanel FillAnswer;
    public FillLetterPanel FillLetter;

    public Button RevealButton, ColorButton, ShareButton;

    public CanvasGroup HintsPanel, GemsPanel, OptionsPanel;
    public WinUI WinUi;

    void Start()
    {
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

        if (GemsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { GemsPanel });
        }

        if (OptionsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { OptionsPanel });
        }
    }

    public void CloseHintPanel()
    {
        ClosePopup(new CanvasGroup[] { HintsPanel, this.GetComponent<CanvasGroup>() });
    }

    public void OpenGemsPanel()
    {
        OpenPopup(this.GetComponent<CanvasGroup>());
        OpenPopup(GemsPanel);

        if (HintsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { HintsPanel });
        }

        if (OptionsPanel.alpha == 1)
        {
            ClosePopup(new CanvasGroup[] { OptionsPanel });
        }

        if (PlayerPrefs.GetInt(System.DateTime.Today.ToShortDateString(), 0) >= 5)
        {
            ShareButton.interactable = false;
            ShareButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;
        }
    }

    public void CloseGemsPanel()
    {
        if (HintsPanel.alpha == 0)
        {
            ClosePopup(new CanvasGroup[] { GemsPanel, this.GetComponent<CanvasGroup>() });
        }
        else
        {
            ClosePopup(GemsPanel);
        }
    }


    public void UseColorHint()
    {
        if (PlayerStats.s_PlayerGems < k_Color)
        {
            ClosePopup(HintsPanel);
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Color;
            PlayerStats.s_ColorHintUsed = PlayerStats.s_ColorHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            ColorButton.interactable = false;
            ColorButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;

            FillAnswer.ClearButtonPressed();

            FillLetter.ColorRequiredTiles();
            FillLetter.DisableNotRequiredTiles();

            ClosePopup(new CanvasGroup[] { HintsPanel, this.GetComponent<CanvasGroup>() });
        }
    }

    public void UseRevealHint()
    {
        if (PlayerStats.s_PlayerGems < k_Reveal)
        {
            ClosePopup(HintsPanel);
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Reveal;
            PlayerStats.s_FillHintUsed = PlayerStats.s_FillHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            RevealButton.interactable = false;
            RevealButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;

            FillAnswer.FillFirstWord();

            ClosePopup(new CanvasGroup[] { HintsPanel, this.GetComponent<CanvasGroup>() });
        }
    }

    public void UseSkipHint()
    {
        if (PlayerStats.s_PlayerGems < k_Skip)
        {
            ClosePopup(HintsPanel);
            OpenGemsPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= k_Skip;

            WinUi.MakeWinVisible();

            ClosePopup(new CanvasGroup[] { HintsPanel, this.GetComponent<CanvasGroup>() });
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

    public void Share()
    {
        SocialShareSheet _shareSheet = new SocialShareSheet();
        _shareSheet.Text = "I've working on F in the B puzzle #" + (PlayerStats.s_CurrentLevel + 1);

#if UNITY_IOS
        _shareSheet.URL = "https://itunes.apple.com/us/app/f-in-the-b/id1328718409?ls=1&mt=8";
#elif UNITY_ANDROID
        _shareSheet.URL = m_shareURL;
#endif
        GemsPanel.alpha = 0;
        _shareSheet.AttachScreenShot();
        GemsPanel.alpha = 1;

        // Show composer
        NPBinding.UI.SetPopoverPointAtLastTouchPosition(); // To show popover at last touch point on iOS. On Android, its ignored.
        NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
    }



    private void FinishedSharing(eShareResult _result)
    {
        if (_result == eShareResult.CLOSED)
        {
            if (PlayerPrefs.GetInt(System.DateTime.Today.ToShortDateString(), 0) < 5)
            {
                PlayerStats.s_PlayerGems += 5;
                PlayerPrefs.SetInt(System.DateTime.Today.ToShortDateString(), PlayerPrefs.GetInt(System.DateTime.Today.ToShortDateString(), 0) + 1);

                if (PlayerPrefs.GetInt(System.DateTime.Today.ToShortDateString(), 0) >= 5)
                {
                    ShareButton.interactable = false;
                    ShareButton.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Disabled Image")).First().enabled = true;
                }
            }
        }
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
