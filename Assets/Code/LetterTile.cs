using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    public void LetterPressed()
    {
        if (CurrentAnswer.s_PlayersAnswerIsNotComplete)
        {
            CurrentAnswer.s_PlayersAttempt += this.GetComponentInChildren<Text>().text;
            this.GetComponent<Button>().interactable = false;
        }
    }
}
