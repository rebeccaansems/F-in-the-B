#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class FintheBEditorItems
{
    [MenuItem("Tools/Check Questions Validity")]
    public static void CheckIfValid()
    {
        string path = "Assets/Resources/Questions.json";
        string questionsString = new StreamReader(path).ReadToEnd().ToString();
        List<QuestionObject> allQuestions = JsonUtility.FromJson<TopQuestionObject>(questionsString).Questions.ToList<QuestionObject>();

        foreach (QuestionObject question in allQuestions)
        {
            foreach (string word in question.Question.Split(' '))
            {
                if (word.Length > 9)
                {
                    Debug.LogError(question.Question);
                }
            }
        }
        Debug.Log("Question Check Complete");
    }

    [MenuItem("Tools/In Game/Reset Gems 100")]
    public static void ResetGems()
    {
        PlayerStats.Instance.ChangeGems(100 - PlayerStats.s_PlayerGems);
    }



    [System.Serializable]
    public class TopQuestionObject
    {
        public QuestionObject[] Questions;
    }

    [System.Serializable]
    public struct QuestionObject
    {
        public string Question;
        public string Hint;
    }
}
#endif
