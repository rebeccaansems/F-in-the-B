using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIOpening : UI
{

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetSceneByBuildIndex(0).IsValid())
        {
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
