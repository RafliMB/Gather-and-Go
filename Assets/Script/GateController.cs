using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using UnityEngine.InputSystem; 

public class GateController : MonoBehaviour
{
    [Header("Pengaturan Kunci")]
    public int jumlahKunciDibutuhkan = 2;
    
    public int kunciTerkumpul { get; private set; } = 0; 

    [Header("Visual Landmark")]
    public GameObject visualLandmarkObject;
    public GameObject completionEffect;

    [Header("Pengaturan UI Notifikasi")]
    public TextMeshProUGUI teksNotifikasi; 

    [Header("Pengaturan UI Selesai")]
    public GameObject panelLevelSelesai;
    public GameObject background;

    [Header("Pengaturan UI Gameplay (HUD)")]
    [Tooltip("Masukkan GameObject yang menampung Teks Kunci dan Tombol Menu agar bisa disembunyikan saat tamat")]
    public GameObject gameplayHUD;
    
    [Header("Pengaturan Pindah Scene")]
    public string namaSceneLevelSelanjutnya = "Level2";
    public string namaSceneMenuLevel = "MenuLevel";
    public string namaSceneMainMenu = "MainMenu";

    private bool playerDiGerbang = false;

    private void Start()
    {        
        if (visualLandmarkObject != null) visualLandmarkObject.SetActive(false);
        if (completionEffect != null) completionEffect.SetActive(false);
        if (teksNotifikasi != null) teksNotifikasi.text = "";
        if (panelLevelSelesai != null) panelLevelSelesai.SetActive(false);
        if (background != null) background.SetActive(false);

        if (gameplayHUD != null) gameplayHUD.SetActive(true);
    }

    private void Update()
    {
        
        if (playerDiGerbang && kunciTerkumpul >= jumlahKunciDibutuhkan)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                SelesaikanLevel();
            }
        }
    }

    public void TambahKunci()
    {
        kunciTerkumpul++;
        Debug.Log("Kunci didapat! Total saat ini: " + kunciTerkumpul + " / " + jumlahKunciDibutuhkan);
        
        UIManager uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }

        if (kunciTerkumpul >= jumlahKunciDibutuhkan)
        {
            AktifkanVisualLandmark();
        }
    }

    private void AktifkanVisualLandmark()
    {
        if (visualLandmarkObject != null) visualLandmarkObject.SetActive(true);
        if (completionEffect != null) completionEffect.SetActive(true);
        Debug.Log("Portal Selesai Level Aktif!");
        
        if (teksNotifikasi != null)
        {
            teksNotifikasi.text = "Semua kunci terkumpul! Pergi ke area merah untuk menyelesaikan level.";
            teksNotifikasi.color = Color.yellow;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDiGerbang = true;
            
            if (kunciTerkumpul < jumlahKunciDibutuhkan)
            {
                int sisa = jumlahKunciDibutuhkan - kunciTerkumpul;
                if (teksNotifikasi != null)
                {
                    teksNotifikasi.text = "Cari " + sisa + " kunci lagi untuk membuka landmark!";
                    teksNotifikasi.color = Color.red;
                }
            }
            else
            {
                if (teksNotifikasi != null)
                {
                    teksNotifikasi.text = "Tekan F untuk menyelesaikan level!";
                    teksNotifikasi.color = Color.green;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDiGerbang = false;

            if (teksNotifikasi != null)
            {                
                if (kunciTerkumpul >= jumlahKunciDibutuhkan)
                {
                    teksNotifikasi.text = "Pergi ke area merah untuk menyelesaikan level.";
                    teksNotifikasi.color = Color.yellow;
                }
                else
                {
                    teksNotifikasi.text = ""; 
                }
            }
        }
    }

    private void SelesaikanLevel()
    {
        Debug.Log("Menampilkan UI Level Selesai...");  

        if (gameplayHUD != null)
        {
            gameplayHUD.SetActive(false); 
        }
        
        if (panelLevelSelesai != null)
        {            
            panelLevelSelesai.SetActive(true);
            background.SetActive(true);
                        
            Time.timeScale = 0f; 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        if (teksNotifikasi != null) teksNotifikasi.text = "";
    }

    public void TombolUlangiLevel()
    {
        Time.timeScale = 1f;
        
        string sceneSekarang = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneSekarang);
    }

    public void TombolLevelSelanjutnya()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(namaSceneLevelSelanjutnya);
    }

    public void TombolMenuLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(namaSceneMenuLevel);
    }

    public void TombolMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(namaSceneMainMenu);
    }
}