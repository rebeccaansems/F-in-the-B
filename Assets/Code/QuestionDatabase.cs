﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionDatabase : MonoBehaviour
{
    public static List<QuestionObject> s_AllQuestions = new List<QuestionObject>();

    public TextAsset QuestionsFile;

    void Awake()
    {
        string questionsString = QuestionsFile.text;
        List<string> questions = questionsString.Split('}').ToList<string>();

        for (int i = 0; i < questions.Count - 1; i++)
        {
            questions[i] += "}";
            s_AllQuestions.Add(JsonUtility.FromJson<QuestionObject>(questions[i]));
        }
    }

    [Serializable]
    public class QuestionObject
    {
        public string Question;
        public string Category;
    }

}