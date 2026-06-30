using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    [Header("Musik Untuk Scene Ini")]
    public AudioClip musikSceneIni;
    private void Start()
    {
        // Saat scene baru dibuka, cari BGMManager utama dan suruh ganti lagu
        if (BGMManager.instance != null && musikSceneIni != null)
        {
            BGMManager.instance.GantiMusik(musikSceneIni);
        }
    }
}