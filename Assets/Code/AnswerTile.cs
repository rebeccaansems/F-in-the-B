using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnswerTile : MonoBehaviour
{
    public bool DeletableTile = false;
    public bool EditableTile = true;
    public int IndexInAnswer = -1;
    public int IndexInWord = -1;
    public int WordIndexInAnswer = -1;
    public GameObject LinkedLetterTile;

    private GameObject gameController;

    public void Start()
    {
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0];

        if (this.GetComponentInChildren<Text>().text == "_")
        {
            WordIndexInAnswer = GetCurrentWord(IndexInAnswer);
            IndexInWord = GetCurrentIndexInWord(IndexInAnswer);
            CurrentAnswer.s_EditableTileParticleSystems[WordIndexInAnswer][IndexInWord] = this.GetComponentsInChildren<ParticleSystem>()[0];
        }
        else
        {
            EditableTile = false;
        }
        this.GetComponent<Button>().interactable = false;
    }

    public void LetterPressed()
    {
        this.GetComponent<PlayAudio>().PlayRandom();

        CurrentAnswer.s_PlayersAttempt = CurrentAnswer.s_PlayersAttempt.Remove(IndexInAnswer, 1).Insert(IndexInAnswer, "_");
        this.GetComponent<Button>().interactable = false;

        LinkedLetterTile.GetComponent<Button>().interactable = true;
        LinkedLetterTile.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
        LinkedLetterTile.GetComponent<LetterTile>().LetterUsed = false;

        gameController.GetComponent<FillAnswerPanel>().AddBlanks();
    }

    public void LinkLetters(GameObject linkLetter)
    {
        LinkedLetterTile = linkLetter;
        LinkedLetterTile.GetComponent<LetterTile>().LinkedAnswerTile = this;

        CheckWordIsFinishedAndCorrect();
    }

    private void CheckWordIsFinishedAndCorrect()
    {
        string[] playerAnswer = new string[CurrentAnswer.s_PlayersCorrectAnswerSeparateWords.Length];
        int totalLength = 0;
        for (int i = 0; i < CurrentAnswer.s_PlayersCorrectAnswerSeparateWords.Length; i++)
        {
            totalLength += CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length;
            playerAnswer[i] = CurrentAnswer.s_PlayersAttempt.Substring(totalLength - CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length, CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length);
        }

        if (!playerAnswer[WordIndexInAnswer].Contains("_") && playerAnswer[WordIndexInAnswer] == CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[WordIndexInAnswer])
        {
            CurrentAnswer.s_BeginningTileParticleSystems[WordIndexInAnswer].Play();
            foreach (ParticleSystem ps in CurrentAnswer.s_EditableTileParticleSystems[WordIndexInAnswer])
            {
                ps.Play();
            }
        }
    }






    private int GetCurrentWord(int index)
    {
        int currentLength = 0;
        for (int i = 0; i < CurrentAnswer.s_PlayersCorrectAnswerSeparateWords.Length; i++)
        {
            currentLength += CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length;
            if (currentLength > index)
            {
                return i;
            }
        }
        return -1;
    }

    private int GetCurrentIndexInWord(int index)
    {
        int currentLength = 0;
        for (int i = 0; i < CurrentAnswer.s_PlayersCorrectAnswerSeparateWords.Length; i++)
        {
            currentLength += CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length;
            if (currentLength > index)
            {
                return (currentLength - CurrentAnswer.s_PlayersCorrectAnswerSeparateWords[i].Length - index) * -1;
            }
        }
        return -1;
    }
}
