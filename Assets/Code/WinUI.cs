using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<CanvasGroup>().interactable = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Time.timeScale = 1;
    }

    public void MakeWinVisible()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().interactable = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Time.timeScale = 0;
    }

    public void NextPuzzlePressed()
    {
        PlayerStats.s_PlayerScore += 2;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Advertisement.Show();
    }
}
