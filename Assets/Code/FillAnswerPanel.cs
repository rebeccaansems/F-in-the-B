using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAnswerPanel : MonoBehaviour
{

    public GameObject[] AnswerRow1, AnswerRow2, AnswerRow3, AnswerRow4;

    private string[] correctAnswer = new string[] { "5 v_____", "in the", "a_______" };

    void Start()
    {
        List<GameObject[]> answerRows = new List<GameObject[]> { AnswerRow1, AnswerRow2, AnswerRow3, AnswerRow4 };
        for (int j = 0; j < answerRows.Count; j++)
        {
            if (j < correctAnswer.Length)
            {
                for (int i = 0; i < correctAnswer[j].Length; i++)
                {
                    answerRows[j][i].GetComponentInChildren<Text>().text = correctAnswer[j][i].ToString();
                }

                for (int i = correctAnswer[j].Length; i < answerRows[j].Length; i++)
                {
                    Destroy(answerRows[j][i]);
                }
            }
            else
            {
                for (int i = 0; i < answerRows[j].Length; i++)
                {
                    Destroy(answerRows[j][i]);
                }
            }
        }
    }
}
