using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public FillAnswerPanel FillAnswer;
    public FillLetterPanel FillLetter;

    public CanvasGroup HintFillPanel, HintColorPanel;

    void Start()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintFillPanel.gameObject);
        ClosePopup(HintColorPanel.gameObject);
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

        if (PlayerStats.s_PlayerScore < 5)
        {
            HintColorPanel.GetComponentsInChildren<Button>().Where(x => x.transform.gameObject.name.Contains("Use")).FirstOrDefault().interactable = false;
        }
        else
        {
            HintColorPanel.GetComponentsInChildren<Button>().Where(x => x.transform.gameObject.name.Contains("Use")).FirstOrDefault().interactable = true;
        }
    }

    public void PressedCloseColorHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintColorPanel.gameObject);
    }

    public void PressedUseColorHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintColorPanel.gameObject);

        PlayerStats.s_PlayerScore -= 5;
        FillLetter.ColorNeededTiles();
    }


    public void PressedFillHint()
    {
        OpenPopup(this.gameObject);
        OpenPopup(HintFillPanel.gameObject);

        if (PlayerStats.s_PlayerScore < 10)
        {
            HintFillPanel.GetComponentsInChildren<Button>().Where(x => x.transform.gameObject.name.Contains("Use")).FirstOrDefault().interactable = false;
        }
        else
        {
            HintFillPanel.GetComponentsInChildren<Button>().Where(x => x.transform.gameObject.name.Contains("Use")).FirstOrDefault().interactable = true;
        }
    }

    public void PressedCloseFillHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintFillPanel.gameObject);
    }

    public void PressedUseFillHint()
    {
        ClosePopup(this.gameObject);
        ClosePopup(HintFillPanel.gameObject);

        PlayerStats.s_PlayerScore -= 10;
        FillAnswer.FillFirstWord();
    }
}
