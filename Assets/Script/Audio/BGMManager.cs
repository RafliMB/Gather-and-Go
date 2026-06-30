using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Ini adalah 'Singleton' agar script lain bisa menemukan BGM dengan mudah
    public static BGMManager instance; 
    
    // Menyimpan komponen AudioSource
    public AudioSource audioSource;

    private void Awake()
    {
        // Mengecek apakah sudah ada BGM Manager lain yang menyala dari scene sebelumnya
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Hancurkan yang baru agar musik tidak double
            return;
        }

        // Jika belum ada, jadikan ini sebagai BGM utama
        instance = this;
        
        // Perintah sakti agar objek ini tidak hancur saat pindah scene
        DontDestroyOnLoad(gameObject);

        // Mengambil komponen AudioSource secara otomatis
        audioSource = GetComponent<AudioSource>();
    }

    public void GantiMusik(AudioClip musikBaru)
    {
        // Cegah musik mengulang dari awal jika scene baru menggunakan musik yang sama
        if (audioSource.clip == musikBaru) 
        {
            return; 
        }

        audioSource.clip = musikBaru;
        audioSource.Play(); // Mainkan musik yang baru
    }    
}