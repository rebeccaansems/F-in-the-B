using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillLetterPanel : MonoBehaviour
{
    public CanvasGroup TileParent;

    private string[] correctAnswer = new string[] { "owels", "lphabet" };

    private const string k_allLetters = "qwertyuiopasdfghjklzxcvbnm";

    void Start()
    {
        string singleCorrectAnswer = string.Join("", correctAnswer);
        int i = 0;
        foreach (Text tile in TileParent.GetComponentsInChildren<Text>())
        {
            if(i < singleCorrectAnswer.Length)
            {
                tile.text = singleCorrectAnswer[i].ToString();
                i++;
            }
            else
            {
                tile.text = k_allLetters[Random.Range(0, 25)].ToString();
            }
        }
    }
}
