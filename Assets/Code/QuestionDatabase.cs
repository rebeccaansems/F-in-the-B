using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class QuestionDatabase : MonoBehaviour
{
    public static List<QuestionObject> s_AllQuestions = new List<QuestionObject>();

    private static bool s_GameStarted;

    void Awake()
    {
        if (s_GameStarted == false)
        {
            string path = "Assets/Resources/Questions.json";
            string questionsString = new StreamReader(path).ReadToEnd().ToString();
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
