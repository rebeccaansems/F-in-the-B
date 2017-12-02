using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FillAnswerPanel : MonoBehaviour
{
    public GameObject Tile, TileParent;

    private string lastGuess = "";
    private string[] splitCorrectAnswer;
    private List<Text> allPossibleOpenings;
    private int numberOfTileRows;

    void Start()
    {
        string correctAnswer = Regex.Replace(CurrentAnswer.s_CorrectAnswer, @"[a-z]", "_");
        splitCorrectAnswer = correctAnswer.Split(' ');

        for (int j = 0; j < splitCorrectAnswer.Length; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject newTile = Instantiate(Tile, TileParent.transform);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 115, -j * 115, 0);
            }
        }

        FillLetters();
        DeleteEmptyTiles();

        numberOfTileRows = TileParent.GetComponentsInChildren<Text>()
            .Where(x => x.transform.parent.GetComponent<AnswerTile>().DeletableTile == false).Count() / 8;

        ShiftTiles();
    }

    private void FillLetters()
    {
        int currentLetter = 0;
        for (int j = 0; j < splitCorrectAnswer.Length; j++)
        {
            if (splitCorrectAnswer[j].Length + (currentLetter % 8) <= 8)
            {
                for (int i = 0; i < splitCorrectAnswer[j].Length; i++)
                {
                    TileParent.transform.GetComponentsInChildren<Text>()[i + currentLetter].text = splitCorrectAnswer[j][i].ToString();
                }
                currentLetter += splitCorrectAnswer[j].Length + 1;
            }
            else
            {
                currentLetter = (int)(System.Math.Ceiling((decimal)currentLetter / 8) * 8);
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
                if (blankCounter == 8)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        TileParent.GetComponentsInChildren<Text>()[i - j].transform.parent.GetComponent<AnswerTile>().DeletableTile = true;
                    }
                    blankCounter = 0;
                }
            }
        }

        foreach (Text tile in TileParent.GetComponentsInChildren<Text>()
            .Where(x => x.transform.parent.GetComponent<AnswerTile>().DeletableTile == true))
        {
            Destroy(tile.transform.parent.gameObject);
        }
    }

    private void ShiftTiles()
    {
        TileParent.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, (8-numberOfTileRows-3) * 115);
    }

    private void Update()
    {
        if (CurrentAnswer.s_PlayersAttempt.Length != lastGuess.Length)
        {
            lastGuess = CurrentAnswer.s_PlayersAttempt;
            this.GetComponent<FillAnswerPanel>().RefillPanel();
        }
    }

    public void RefillPanel()
    {
        allPossibleOpenings = TileParent.GetComponentsInChildren<Text>().Where(x => x.text == "_").ToList();
        if (allPossibleOpenings.Count != 0)
        {
            allPossibleOpenings.First().text = CurrentAnswer.s_PlayersAttempt[CurrentAnswer.s_PlayersAttempt.Length - 1].ToString();

            if (allPossibleOpenings.Count == 1)
            {
                CurrentAnswer.s_PlayersAnswerIsNotComplete = false;
            }
        }
    }

    public void ClearButtonPressed()
    {
        this.GetComponent<FillLetterPanel>().MakeAllButtonsInteractable();
        FillLetters();
        CurrentAnswer.s_PlayersAnswerIsNotComplete = true;
    }
}
