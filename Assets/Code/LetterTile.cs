using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0];
    }

    public void LetterPressed()
    {
        if (CurrentAnswer.s_PlayersAnswerIsNotComplete)
        {
            gameController.GetComponent<FillAnswerPanel>().CurrentLetterTile = this.transform.gameObject;
            if (CurrentAnswer.s_PlayersAttempt.Contains("_"))
            {
                CurrentAnswer.s_PlayersAttempt = ReplaceFirst(CurrentAnswer.s_PlayersAttempt, "_", this.GetComponentInChildren<Text>().text);
            }
            else
            {
                CurrentAnswer.s_PlayersAttempt += this.GetComponentInChildren<Text>().text;
            }
            this.GetComponent<Button>().interactable = false;
        }
    }

    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}
