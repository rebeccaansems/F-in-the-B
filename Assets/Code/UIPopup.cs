using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public FillAnswerPanel FillAnswer;
    public FillLetterPanel FillLetter;

    public Button HintFillButton, HitnColorButton;

    public CanvasGroup HintFillPanel, HintColorPanel, NECPanel;

    void Start()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintFillPanel.gameObject);
        ClosePopup(HintColorPanel.gameObject);
        ClosePopup(NECPanel.gameObject);

        if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '0')
        {
            HintFillButton.interactable = true;
            HintFillButton.GetComponentsInChildren<Image>()[1].enabled = false;
        }
        else if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            HintFillButton.interactable = false;
            HintFillButton.GetComponentsInChildren<Image>()[1].enabled = true;
        }

        if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '0')
        {
            HitnColorButton.interactable = true;
            HitnColorButton.GetComponentsInChildren<Image>()[1].enabled = false;
        }
        else if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            HitnColorButton.interactable = false;
            HitnColorButton.GetComponentsInChildren<Image>()[1].enabled = true;
        }
    }

    private void OpenPopup(GameObject go)
    {
        go.GetComponent<CanvasGroup>().alpha = 1;
        go.GetComponent<CanvasGroup>().interactable = true;
        go.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Time.timeScale = 0;
    }

    private void ClosePopup(GameObject go)
    {
        go.GetComponent<CanvasGroup>().alpha = 0;
        go.GetComponent<CanvasGroup>().interactable = false;
        go.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Time.timeScale = 1;
    }


    public void PressedColorHint()
    {
        OpenPopup(this.gameObject);
        OpenPopup(HintColorPanel.gameObject);
    }

    public void PressedCloseColorHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintColorPanel.gameObject);
    }

    public void PressedUseColorHint()
    {
        if (PlayerStats.s_PlayerGems < 5)
        {
            OpenNECPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= 5;
            PlayerStats.s_ColorHintUsed = PlayerStats.s_ColorHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            FillLetter.ColorNeededTiles();
            ClosePopup(HintColorPanel.gameObject);
            ClosePopup(this.gameObject);
        }
    }


    public void PressedFillHint()
    {
        OpenPopup(this.gameObject);
        OpenPopup(HintFillPanel.gameObject);
    }

    public void PressedCloseFillHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintFillPanel.gameObject);
    }

    public void PressedUseFillHint()
    {
        if (PlayerStats.s_PlayerGems < 10)
        {
            OpenNECPanel();
        }
        else
        {
            PlayerStats.s_PlayerGems -= 10;
            PlayerStats.s_FillHintUsed = PlayerStats.s_FillHintUsed.Remove(PlayerStats.s_CurrentLevel, 1).Insert(PlayerStats.s_CurrentLevel, "1");

            FillAnswer.FillFirstWord();
            ClosePopup(HintFillPanel.gameObject);
            ClosePopup(this.gameObject);
        }
    }


    public void OpenNECPanel()
    {
        OpenPopup(NECPanel.gameObject);
    }

    public void PressedCloseNECPanel()
    {
        ClosePopup(NECPanel.gameObject);
    }

    public void PressedUseNEC()
    {
        ClosePopup(NECPanel.gameObject);

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
                PlayerStats.s_PlayerGems += 5;
                break;
            case ShowResult.Failed:
                break;
        }
    }
}
