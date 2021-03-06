﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LetterTile : MonoBehaviour
{
    public bool IsRequiredForAnswer = false, IsPartOfFirstWord = false, LetterUsed = false;
    public Color AlternateColorLight, AlternateColorDark;
    public AnswerTile LinkedAnswerTile;

    private GameObject gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0];

        this.GetComponent<Button>().interactable = true;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
    }

    public void LetterPressed()
    {
        if (LetterUsed)
        {
            LetterUsed = false;
            LinkedAnswerTile.LetterPressed();

            if (PlayerStats.s_TutorialOn == 1)
            {
                StartCoroutine(Tutorial.Instance.ShowTutorial(5));
            }
        }
        else if (CurrentAnswer.s_PlayersAnswerIsNotComplete)
        {
            LetterUsed = true;

            if (PlayerStats.s_TutorialOn == 1)
            {
                StartCoroutine(Tutorial.Instance.ShowTutorial(1));

                if (Tutorial.Instance.CurrentTut == 2)
                {
                    StartCoroutine(Tutorial.Instance.ShowTutorial(3));
                }
                else if (Tutorial.Instance.CurrentTut == 3)
                {
                    StartCoroutine(Tutorial.Instance.ShowTutorial(4));
                }
            }

            this.GetComponent<PlayAudio>().PlayRandom();
            if (CurrentAnswer.s_PlayersAttempt.Contains("_"))
            {
                ReplaceFirst(CurrentAnswer.s_PlayersAttempt, "_", this.GetComponentInChildren<Text>().text);
            }
            else
            {
                CurrentAnswer.s_PlayersAttempt += this.GetComponentInChildren<Text>().text;
            }

            this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
            gameController.GetComponent<FillAnswerPanel>().RemoveBlanks(this.transform.gameObject);
        }
    }


    private void ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        CurrentAnswer.s_PlayersAttempt = text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    public void SwapToAlternateColorScheme()
    {
        this.GetComponent<Image>().color = AlternateColorLight;
        this.GetComponentsInChildren<Image>().Where(x => x.name == "Background Image").First().color = AlternateColorDark;
    }

    public void EnableTile()
    {
        this.GetComponent<Button>().interactable = true;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
    }

    public void HardDisable()
    {
        this.GetComponent<Button>().interactable = false;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
    }
}
