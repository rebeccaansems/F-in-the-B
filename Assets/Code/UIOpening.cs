using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIOpening : UI
{

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.UnloadSceneAsync(0);
    }
}
