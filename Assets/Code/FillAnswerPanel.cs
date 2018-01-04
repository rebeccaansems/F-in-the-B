﻿using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FillAnswerPanel : MonoBehaviour
{
    public GameObject Tile, TileParent, CurrentLetterTile;

    private string lastGuess = "";
    private string[] splitCorrectAnswer;

    private int tilesPerRow = 9;

    void Start()
    {
        string correctAnswer = Regex.Replace(CurrentAnswer.s_CorrectAnswer, @"[a-z]", "_");
        splitCorrectAnswer = correctAnswer.Split(' ');

        for (int j = 0; j < splitCorrectAnswer.Length + 1; j++)
        {
            for (int i = 0; i < tilesPerRow; i++)
            {
                GameObject newTile = Instantiate(Tile, TileParent.transform);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 100, -j * 100, 0);
            }
        }

        FillLetters();
        DeleteEmptyTiles();

        if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            FillFirstWord();
        }
    }

    private void FillLetters()
    {
        int currentLetter = 0;
        int editableCount = 0;
        for (int j = 0; j < splitCorrectAnswer.Length; j++)
        {
            if (splitCorrectAnswer[j].Length + (currentLetter % tilesPerRow) <= tilesPerRow)
            {
                for (int i = 0; i < splitCorrectAnswer[j].Length; i++)
                {
                    TileParent.transform.GetComponentsInChildren<Text>()[i + currentLetter].text = splitCorrectAnswer[j][i].ToString();
                    if (splitCorrectAnswer[j][i] == '_')
                    {
                        TileParent.transform.GetComponentsInChildren<AnswerTile>()[i + currentLetter].IndexInAnswer = editableCount;
                        editableCount++;
                    }
                }
                currentLetter += splitCorrectAnswer[j].Length + 1;
            }
            else
            {
                currentLetter = (int)(System.Math.Ceiling((decimal)currentLetter / tilesPerRow) * tilesPerRow);
                j--;
            }
        }
    }

    private void DeleteEmptyTiles()
    {
        int blankCounter = 0;
        for (int i = 0; i < TileParent.GetComponentsInChildren<Text>().Length; i++)
        {
            if (TileParent.GetComponentsInChildren<Text>()[i].text != string.Empty)
            {
                blankCounter = 0;
            }
            else
            {
                blankCounter++;
                if (blankCounter == tilesPerRow)
                {
                    for (int j = i - tilesPerRow + 1; j < TileParent.GetComponentsInChildren<Text>().Length; j++)
                    {
                        TileParent.GetComponentsInChildren<Text>()[j].transform.parent.GetComponent<AnswerTile>().DeletableTile = true;
                    }
                    i = TileParent.GetComponentsInChildren<Text>().Length;
                }
            }
        }

        foreach (Text tile in TileParent.GetComponentsInChildren<Text>()
            .Where(x => x.transform.parent.GetComponent<AnswerTile>().DeletableTile == true))
        {
            Destroy(tile.transform.parent.gameObject);
        }
    }

    public void RemoveBlanks(GameObject currLetterTile)
    {
        List<Text> allPossibleOpenings = TileParent.GetComponentsInChildren<Text>().Where(x => x.text == "_").ToList();
        if (allPossibleOpenings.Count != 0)
        {
            allPossibleOpenings.First().transform.parent.GetComponent<Button>().interactable = true;
            if (CurrentAnswer.s_PlayersAttempt.Length > allPossibleOpenings.First().transform.parent.GetComponent<AnswerTile>().IndexInAnswer)
            {
                allPossibleOpenings.First().text = CurrentAnswer.s_PlayersAttempt[allPossibleOpenings.First().transform.parent.GetComponent<AnswerTile>().IndexInAnswer].ToString();
                allPossibleOpenings.First().transform.parent.GetComponent<AnswerTile>().LinkLetters(currLetterTile);

                if (allPossibleOpenings.Count == 1)
                {
                    CurrentAnswer.s_PlayersAnswerIsNotComplete = false;
                }
            }
        }
        else
        {
            CurrentAnswer.s_PlayersAnswerIsNotComplete = false;
        }
    }

    public void AddBlanks()
    {
        List<AnswerTile> allPossibleEditables = TileParent.GetComponentsInChildren<AnswerTile>().Where(x => x.EditableTile).ToList();

        for (int i = 0; i < CurrentAnswer.s_PlayersAttempt.Length; i++)
        {
            allPossibleEditables[i].gameObject.GetComponentInChildren<Text>().text = CurrentAnswer.s_PlayersAttempt[i].ToString();
        }

        CurrentAnswer.s_PlayersAnswerIsNotComplete = true;
    }

    public void ClearButtonPressed()
    {
        this.GetComponent<FillLetterPanel>().MakeAllButtonsInteractable();
        FillLetters();

        CurrentAnswer.s_PlayersAnswerIsNotComplete = true;
        CurrentAnswer.s_PlayersAttempt = "";
        lastGuess = "";
    }

    public void FillFirstWord()
    {
        ClearButtonPressed();

        splitCorrectAnswer = CurrentAnswer.s_CorrectAnswer.Split(' ');
        string editedCorrectAnswer = "";
        for (int i=2; i< splitCorrectAnswer.Length; i++)
        {
            editedCorrectAnswer += Regex.Replace(splitCorrectAnswer[i], @"[A-Z,0-9]", string.Empty);
            splitCorrectAnswer[i] = Regex.Replace(splitCorrectAnswer[i], @"[a-z]", "_");
        }

        CurrentAnswer.s_PlayersCorrectAnswer = editedCorrectAnswer;
        FillLetters();

        foreach (AnswerTile tile in TileParent.GetComponentsInChildren<AnswerTile>().Where(x => x.EditableTile).Where(x => x.GetComponentsInChildren<Text>()[0].text != "_"))
        {
            tile.EditableTile = false;
        }
        this.GetComponent<FillLetterPanel>().DisableAllFirstWordRequiredTiles();
    }
}
