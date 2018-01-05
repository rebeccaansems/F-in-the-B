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

    public static int s_SFXAudio, s_MusicAudio;

    public Text PlayerGems;

    public ParticleSystem LoseGemsParticleSystem, GainGemsParticleSystem;

    private int prevLevel = -1;
    private bool playAudio = true;

    private static PlayerStats _instance;

    public static PlayerStats Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
        s_SFXAudio = PlayerPrefs.GetInt("SFXAudio", 1);
        s_MusicAudio = PlayerPrefs.GetInt("MusicAudio", 1);

#if UNITY_EDITOR
        s_PlayerGems = PlayerPrefs.GetInt("PlayerGem", 10000);
#else
        s_PlayerGems = PlayerPrefs.GetInt("PlayerGem", 50);
#endif
        s_CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        PlayerGems.text = s_PlayerGems.ToString();

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

    public void ChangeGems(int amount)
    {
        s_PlayerGems += amount;

        if (amount > 0)
        {
            GainGemsParticleSystem.Play();
            StartCoroutine(UpdateScore(GainGemsParticleSystem));
        }
        else
        {
            LoseGemsParticleSystem.Play();
            StartCoroutine(UpdateScore(LoseGemsParticleSystem));
        }

        PlayerPrefs.SetInt("PlayerGem", s_PlayerGems);
    }

    private void Update()
    {
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
        PlayerGems.text = s_PlayerGems.ToString();
        playAudio = true;
    }
}
