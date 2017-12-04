using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerTile : MonoBehaviour
{
    public bool DeletableTile = false;
    public bool EditableTile = true;
    public int IndexInAnswer = 0;
    public GameObject LinkedLetterTile;

    public void Start()
    {
        if (this.GetComponentInChildren<Text>().text != "_")
        {
            EditableTile = false;
        }
        this.GetComponent<Button>().interactable = false;
    }

    public void LetterPressed()
    {
        CurrentAnswer.s_PlayersAttempt = CurrentAnswer.s_PlayersAttempt.Remove(IndexInAnswer, 1).Insert(IndexInAnswer, "_");
        this.GetComponent<Button>().interactable = false;

        LinkedLetterTile.GetComponent<Button>().interactable = true;
        LinkedLetterTile.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
    }
}
