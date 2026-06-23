using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadSceneBaru(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Aplikasi keluar");
        Application.Quit();
    }
}
