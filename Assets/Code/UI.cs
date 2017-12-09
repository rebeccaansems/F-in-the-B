using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void OpenPopup(CanvasGroup go)
    {
        go.alpha = 1;
        go.interactable = true;
        go.blocksRaycasts = true;

        Time.timeScale = 0;
    }

    public void ClosePopup(CanvasGroup go)
    {
        go.interactable = false;
        go.blocksRaycasts = false;
        go.alpha = 0;

        Time.timeScale = 1;
    }
}
