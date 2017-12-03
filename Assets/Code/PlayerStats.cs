using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerGems;

    public Text PlayerGems;

    private int prevGem = 0;

    private void Start()
    {
        s_PlayerGems = PlayerPrefs.GetInt("PlayerGem", 100);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
    }

    private void Update()
    {
        if (prevGem != s_PlayerGems)
        {
            PlayerGems.text = s_PlayerGems.ToString();
            prevGem = s_PlayerGems;
            PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
        }
    }
}
