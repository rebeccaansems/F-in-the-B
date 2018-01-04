using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionDatabase : MonoBehaviour
{
    public static List<QuestionObject> s_AllQuestions = new List<QuestionObject>();

    public TextAsset QuestionsFile;

    private static bool s_GameStarted;

    void Awake()
    {
        if (s_GameStarted == false)
        {
            string questionsString = QuestionsFile.text;
            s_AllQuestions = JsonUtility.FromJson<TopQuestionObject>(questionsString).Questions.ToList<QuestionObject>();
            s_GameStarted = true;
        }
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
