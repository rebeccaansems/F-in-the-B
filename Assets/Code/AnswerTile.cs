﻿using System.Linq;
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
        LinkedLetterTile.GetComponent<LetterTile>().LetterUsed = false;
    }

    public void LinkLetters(GameObject linkLetter)
    {
        LinkedLetterTile = linkLetter;
        LinkedLetterTile.GetComponent<LetterTile>().LinkedAnswerTile = this;
    }
}
