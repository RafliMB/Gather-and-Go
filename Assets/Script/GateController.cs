using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk berpindah level

public class GateController : MonoBehaviour
{
    [Header("Pengaturan Kunci")]
    public int jumlahKunciDibutuhkan = 2; // Sesuai permintaan Level 2
    private int kunciTerkumpul = 0;

    [Header("Visual Landmark (Ganti Bola Merah)")]
    // Seret objek Landmark visual (Portal/Altar menyala) Anda ke sini di Inspector
    public GameObject visualLandmarkObject;

    // Tambahkan objek Particle System jika ingin efek pilar cahaya tambahan
    public GameObject completionEffect; 

    private bool playerDiGerbang = false;

    private void Start()
    {
        // Pastikan landmark dan efek mati saat game mulai
        if (visualLandmarkObject != null) visualLandmarkObject.SetActive(false);
        if (completionEffect != null) completionEffect.SetActive(false);
    }

    public void TambahKunci()
    {
        kunciTerkumpul++;
        Debug.Log("Kunci didapat! Total saat ini: " + kunciTerkumpul + " / " + jumlahKunciDibutuhkan);

        // --- JIKA KUNCI SUDAH 2, AKTIFKAN VISUAL LANDMARK ---
        if (kunciTerkumpul >= jumlahKunciDibutuhkan)
        {
            AktifkanVisualLandmark();
        }
    }

    private void Update()
    {
        // Player di area, kunci lengkap, tekan F
        if (playerDiGerbang && kunciTerkumpul >= jumlahKunciDibutuhkan && UnityEngine.InputSystem.Keyboard.current.fKey.wasPressedThisFrame)
        {
            SelesaikanLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDiGerbang = true;
            Debug.Log("Berdiri di area penyelesaikan level.");

            if (kunciTerkumpul < jumlahKunciDibutuhkan)
            {
                int sisa = jumlahKunciDibutuhkan - kunciTerkumpul;
                Debug.Log("Cari " + sisa + " kunci lagi untuk membuka landmark!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDiGerbang = false;
        }
    }

    // Fungsi menyalakan efek/portal (menggantikan bola merah statis)
    private void AktifkanVisualLandmark()
    {
        if (visualLandmarkObject != null) visualLandmarkObject.SetActive(true);
        if (completionEffect != null) completionEffect.SetActive(true);
        Debug.Log("Portal Selesai Level Aktif! Silakan menuju area dan tekan F.");
    }

    private void SelesaikanLevel()
    {
        Debug.Log("Selamat! Level Selesai. Memuat Level Berikutnya...");
        
        // Masukkan nama Scene Level 3 atau Scene Main Menu Anda di bawah ini:
        // SceneManager.LoadScene("NamaSceneLevel3");
    }
}