using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int s_PlayerGems;
    public static int s_CurrentLevel;

    public static string s_ColorHintUsed;
    public static string s_FillHintUsed;

    public static float s_PlayerStartPuzzleTime;

    public static bool s_ScoreShouldUpdate;

    public Text PlayerGems;

    public ParticleSystem LoseGemsParticleSystem, GainGemsParticleSystem;

    private static int s_PrevGem = -1;
    private int prevLevel = -1;
    private bool playAudio = true;

    private void Awake()
    {
        s_ScoreShouldUpdate = true;

        s_PlayerGems = PlayerPrefs.GetInt("PlayerGem", 10000);
        s_CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        if (s_PrevGem != -1)
        {
            PlayerGems.text = s_PrevGem.ToString();
            GainGemsParticleSystem.Play();
            GainGemsParticleSystem.gameObject.GetComponent<PlayAudio>().Play();
            UpdateScore(GainGemsParticleSystem);
        }
        else
        {
            PlayerGems.text = s_PlayerGems.ToString();
            s_PrevGem = s_PlayerGems;
        }

        s_ColorHintUsed = PlayerPrefs.GetString("ColorHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));
        s_FillHintUsed = PlayerPrefs.GetString("FillHintUsed", new string('0', QuestionDatabase.s_AllQuestions.Count));

        if (s_ColorHintUsed.Length < QuestionDatabase.s_AllQuestions.Count)
        {
            s_ColorHintUsed += new string('0', QuestionDatabase.s_AllQuestions.Count - s_ColorHintUsed.Length);
            s_FillHintUsed += new string('0', QuestionDatabase.s_AllQuestions.Count - s_FillHintUsed.Length);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
        PlayerPrefs.SetInt("CurrentLevel", s_CurrentLevel);

        PlayerPrefs.SetFloat("PlayerTimeOnPuzzle", Time.realtimeSinceStartup - s_PlayerStartPuzzleTime + PlayerPrefs.GetFloat("PlayerTimeOnPuzzle", 0f));

        PlayerPrefs.SetString("ColorHintUsed", s_ColorHintUsed);
        PlayerPrefs.SetString("FillHintUsed", s_FillHintUsed);
    }

    private void Update()
    {
        if (s_PrevGem != s_PlayerGems && s_ScoreShouldUpdate)
        {
            if (!(LoseGemsParticleSystem.isPlaying && GainGemsParticleSystem.isPlaying))
            {
                if (s_PrevGem > s_PlayerGems)
                {
                    LoseGemsParticleSystem.Play();
                    StartCoroutine(UpdateScore(LoseGemsParticleSystem));
                }
                else
                {
                    GainGemsParticleSystem.Play();
                    StartCoroutine(UpdateScore(GainGemsParticleSystem));
                }
            }
            PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
        }

        if (prevLevel != s_CurrentLevel)
        {
            prevLevel = s_CurrentLevel;
            PlayerPrefs.SetInt("CurrentLevel", s_CurrentLevel);
        }
    }

    IEnumerator UpdateScore(ParticleSystem particleSystem)
    {
        if (playAudio)
        {
            particleSystem.gameObject.GetComponent<PlayAudio>().Play();
            playAudio = false;
        }
        yield return new WaitForSeconds(particleSystem.main.duration / 2);
        s_PrevGem = s_PlayerGems;
        PlayerGems.text = s_PlayerGems.ToString();
        playAudio = true;
    }
}
