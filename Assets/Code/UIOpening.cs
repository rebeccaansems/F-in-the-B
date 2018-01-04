using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIOpening : UI
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
