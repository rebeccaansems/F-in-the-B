﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FillLetterPanel : MonoBehaviour
{
    public GameObject Tile, TileParent;

    private const string k_allLetters = "qwertyuiopasdfghjklzxcvbnm";

    void Start()
    {
        string singleCorrectAnswer = string.Join("", CurrentAnswer.s_CorrectAnswerLetters);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject newTile = Instantiate(Tile, TileParent.transform);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 150, -j * 150, 0);
            }
        }

        int counter = 0;
        System.Random r = new System.Random();
        foreach (Text tile in TileParent.transform.GetComponentsInChildren<Text>().OrderBy(x => r.Next()))
        {
            if (counter < singleCorrectAnswer.Length)
            {
                tile.text = singleCorrectAnswer[counter].ToString();
                tile.GetComponentInParent<LetterTile>().isRequiredForAnswer = true;
            }
            else
            {
                tile.text = k_allLetters[Random.Range(0, 25)].ToString();
                tile.GetComponentInParent<LetterTile>().isRequiredForAnswer = false;
            }
            counter++;
        }

        if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            ColorNeededTiles();
        }
    }

    public void MakeAllButtonsInteractable()
    {
        var turnedOffButtons = TileParent.transform.GetComponentsInChildren<Button>().Where(x => x.interactable == false);
        foreach (Button button in turnedOffButtons)
        {
            button.interactable = true;
            button.gameObject.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
        }
    }

    public void ColorNeededTiles()
    {
        foreach (Text tile in TileParent.transform.GetComponentsInChildren<Text>().Where(x => x.GetComponentInParent<LetterTile>().isRequiredForAnswer))
        {
            tile.color = Color.red;
        }
    }
}
