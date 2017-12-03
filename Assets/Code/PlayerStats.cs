using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerScore;

    private void Start()
    {
        s_PlayerScore = PlayerPrefs.GetInt("PlayerScore", 100);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerScore", s_PlayerScore);
    }
}
