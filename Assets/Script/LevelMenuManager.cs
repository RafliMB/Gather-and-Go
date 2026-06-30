using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [Header("Daftar Tombol Level")]
    [Tooltip("Masukkan tombol secara berurutan: Level 1, Level 2, Level 3, dst")]
    public Button[] tombolLevel;

    [Header("Daftar Ikon Gembok")]
    [Tooltip("Masukkan objek Ikon Gembok yang ada di dalam masing-masing tombol")]
    public GameObject[] ikonGembok;

    private void Start()
    {
        int levelTerbuka = PlayerPrefs.GetInt("LevelTerbuka", 1);

        for (int i = 0; i < tombolLevel.Length; i++)
        {
            if (i + 1 > levelTerbuka)
            {
                tombolLevel[i].interactable = false;

                if (i < ikonGembok.Length && ikonGembok[i] != null)
                {
                    ikonGembok[i].SetActive(true);
                }
            }
            else
            {
                tombolLevel[i].interactable = true;

                if (i < ikonGembok.Length && ikonGembok[i] != null)
                {
                    ikonGembok[i].SetActive(false);
                }
            }
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("LevelTerbuka");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}