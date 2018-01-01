using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
            gameController.GetComponent<FillAnswerPanel>().CurrentLetterTile = this.transform.gameObject;

            if (CurrentAnswer.s_PlayersAttempt.Contains("_"))
            {
                CurrentAnswer.s_PlayersAttempt = ReplaceFirst(CurrentAnswer.s_PlayersAttempt, "_", this.GetComponentInChildren<Text>().text);
            }
            else
            {
                CurrentAnswer.s_PlayersAttempt += this.GetComponentInChildren<Text>().text;
            }
            this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
        }
    }


    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    public void SwapToAlternateColorScheme()
    {
        this.GetComponent<Image>().color = AlternateColorLight;
        this.GetComponentsInChildren<Image>().Where(x => x.name == "Background Image").First().color = AlternateColorDark;
    }

    public void DisableLetter()
    {
        this.GetComponent<Button>().interactable = false;
        this.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Used")).First().enabled = true;
    }
}
