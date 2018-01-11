using System.Collections.Generic;
using System.Linq;
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
                tile.GetComponentInParent<LetterTile>().IsRequiredForAnswer = true;

                tile.GetComponentInParent<LetterTile>().IsPartOfFirstWord = false;
                if (counter < CurrentAnswer.s_CorrectAnswerLetters[0].Length)
                {
                    tile.GetComponentInParent<LetterTile>().IsPartOfFirstWord = true;
                }
            }
            else
            {
                tile.text = k_allLetters[Random.Range(0, 25)].ToString();
                tile.GetComponentInParent<LetterTile>().IsRequiredForAnswer = false;
                tile.GetComponentInParent<LetterTile>().IsPartOfFirstWord = false;
            }
            counter++;
        }

        if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            ColorRequiredTiles();
            DisableNotRequiredTiles();
        }

        if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            DisableAllFirstWordRequiredTiles();
        }
    }

    public void MakeAllButtonsInteractable()    
    {
        foreach (Button button in TileParent.transform.GetComponentsInChildren<Button>())
        {
            button.gameObject.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
            button.GetComponent<LetterTile>().LetterUsed = false;
            button.interactable = true;
        }

        if (PlayerStats.s_ColorHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            ColorRequiredTiles();
            DisableNotRequiredTiles();
        }

        if (PlayerStats.s_FillHintUsed[PlayerStats.s_CurrentLevel] == '1')
        {
            DisableAllFirstWordRequiredTiles();
        }
    }

    public void ColorRequiredTiles()
    {
        foreach (LetterTile tile in TileParent.transform.GetComponentsInChildren<LetterTile>().Where(x => x.IsRequiredForAnswer))
        {
            tile.SwapToAlternateColorScheme();
        }
    }

    public void DisableNotRequiredTiles()
    {
        foreach (LetterTile tile in TileParent.transform.GetComponentsInChildren<LetterTile>().Where(x => !x.IsRequiredForAnswer))
        {
            tile.HardDisable();
        }
    }

    public void DisableAllFirstWordRequiredTiles()
    {
        foreach (LetterTile tile in TileParent.transform.GetComponentsInChildren<LetterTile>().Where(x => x.IsPartOfFirstWord))
        {
            tile.HardDisable();
        }
    }
}
