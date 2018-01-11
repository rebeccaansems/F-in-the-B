using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class LetterTile : MonoBehaviour
{
    public bool IsRequiredForAnswer = false, IsPartOfFirstWord = false, LetterUsed = false;
    public Color AlternateColorLight, AlternateColorDark;
    public AnswerTile LinkedAnswerTile;

    private GameObject gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0];

        this.GetComponent<Button>().interactable = true;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
    }

    public void LetterPressed()
    {
        if (LetterUsed)
        {
            LetterUsed = false;
            LinkedAnswerTile.LetterPressed();
        }
        else if (CurrentAnswer.s_PlayersAnswerIsNotComplete)
        {
            LetterUsed = true;

            this.GetComponent<PlayAudio>().PlayRandom();
            if (CurrentAnswer.s_PlayersAttempt.Contains("_"))
            {
                ReplaceFirst(CurrentAnswer.s_PlayersAttempt, "_", this.GetComponentInChildren<Text>().text);
            }
            else
            {
                CurrentAnswer.s_PlayersAttempt += this.GetComponentInChildren<Text>().text;

                if (CurrentAnswer.s_PlayersAttempt[CurrentAnswer.s_PlayersAttempt.Length - 1] ==
                    CurrentAnswer.s_PlayersCorrectAnswer[CurrentAnswer.s_PlayersAttempt.Length - 1])
                {
                    SoftDisable();
                }
            }

            this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
            gameController.GetComponent<FillAnswerPanel>().RemoveBlanks(this.transform.gameObject);
        }
    }


    private void ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);

        CurrentAnswer.s_PlayersAttempt = text.Substring(0, pos) + replace + text.Substring(pos + search.Length);

        if (CurrentAnswer.s_PlayersAttempt[pos] == CurrentAnswer.s_PlayersCorrectAnswer[pos])
        {
            SoftDisable();
        }
    }

    public void SwapToAlternateColorScheme()
    {
        this.GetComponent<Image>().color = AlternateColorLight;
        this.GetComponentsInChildren<Image>().Where(x => x.name == "Background Image").First().color = AlternateColorDark;
    }

    public void EnableTile()
    {
        this.GetComponent<Button>().interactable = true;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = false;
    }

    public void HardDisable()
    {
        this.GetComponent<Button>().interactable = false;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
    }

    private void SoftDisable()
    {
        this.GetComponent<Button>().interactable = false;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
    }
}
