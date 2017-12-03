using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerScore;

    public Text PlayerScore;

    private int prevScore = 0;

    private void Start()
    {
        s_PlayerScore = PlayerPrefs.GetInt("PlayerScore", 100);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerScore", s_PlayerScore);
    }

    private void Update()
    {
        if(prevScore != s_PlayerScore)
        {
            PlayerScore.text = s_PlayerScore.ToString();
            prevScore = s_PlayerScore;
        }
    }
}
