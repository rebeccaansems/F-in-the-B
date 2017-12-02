using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FillAnswerPanel : MonoBehaviour
{
    public GameObject Tile, TileParent;

    private string correctAnswer = "5 Vowels IN THE Alphabet";

    void Start()
    {
        correctAnswer = Regex.Replace(correctAnswer, @"[a-z]", "_");
        string[] splitCorrectAnswer = correctAnswer.Split(' ');

        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject newTile = Instantiate(Tile, TileParent.transform);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 115, -j * 115, 0);
            }
        }

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
}
