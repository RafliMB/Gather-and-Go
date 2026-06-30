using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using UnityEngine.InputSystem; 

public class GateController : MonoBehaviour
{
    [Header("Pengaturan Progress Level")]
    [Tooltip("Isi dengan angka level ini. Misal ini Level 1, maka isi 1.")]
    public int levelSaatIni = 1;

    [Header("Pengaturan Waktu & Bintang")]
    public float waktuMaksimal = 120f; 
    public float batasWaktuBintang3 = 60f; 
    public float batasWaktuBintang2 = 30f; 
    
    public TextMeshProUGUI teksTimerHUD; 
    public GameObject[] bintangMenyala; 

    private float sisaWaktu;
    private bool levelSudahSelesai = false;

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

        sisaWaktu = waktuMaksimal;
        foreach (GameObject bintang in bintangMenyala)
        {
            if (bintang != null) bintang.SetActive(false);
        }
    }

    private void Update()
    {
        if (!levelSudahSelesai && sisaWaktu > 0)
        {
            sisaWaktu -= Time.deltaTime;
            UpdateTeksTimer();

            if (sisaWaktu <= 0)
            {
                sisaWaktu = 0;
                // Logika Game Over bisa ditaruh di sini
            }
        }

        if (playerDiGerbang && kunciTerkumpul >= jumlahKunciDibutuhkan && !levelSudahSelesai)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                SelesaikanLevel();
            }
        }
    }

    private void UpdateTeksTimer()
    {
        if (teksTimerHUD != null)
        {
            int menit = Mathf.FloorToInt(sisaWaktu / 60);
            int detik = Mathf.FloorToInt(sisaWaktu % 60);
            teksTimerHUD.text = string.Format("{0:00}:{1:00}", menit, detik);
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
        
        // Menghentikan hitung mundur timer
        levelSudahSelesai = true;

        // 1. Simpan Progress Level Terbuka
        int levelTerbuka = PlayerPrefs.GetInt("LevelTerbuka", 1);
        if (levelSaatIni >= levelTerbuka)
        {
            PlayerPrefs.SetInt("LevelTerbuka", levelSaatIni + 1);
            Debug.Log("Level " + (levelSaatIni + 1) + " Terbuka!");
        }
        
        // 2. Hitung dan Simpan Perolehan Bintang
        int jumlahBintang = 1; 
        if (sisaWaktu >= batasWaktuBintang3)
        {
            jumlahBintang = 3;
        }
        else if (sisaWaktu >= batasWaktuBintang2)
        {
            jumlahBintang = 2;
        }

        string namaKeyBintang = "Level" + levelSaatIni + "_Bintang";
        int bintangTersimpan = PlayerPrefs.GetInt(namaKeyBintang, 0);
        if (jumlahBintang > bintangTersimpan)
        {
            PlayerPrefs.SetInt(namaKeyBintang, jumlahBintang);
        }
        
        // Simpan semua PlayerPrefs sekaligus
        PlayerPrefs.Save();

        // 3. Matikan UI Gameplay
        if (gameplayHUD != null)
        {
            gameplayHUD.SetActive(false); 
        }
        
        // 4. Nyalakan Panel Selesai & Bintang
        if (panelLevelSelesai != null)
        {            
            panelLevelSelesai.SetActive(true);
            if (background != null) background.SetActive(true);
            
            // Nyalakan bintang sesuai jumlah
            for (int i = 0; i < jumlahBintang; i++)
            {
                if (i < bintangMenyala.Length && bintangMenyala[i] != null)
                {
                    bintangMenyala[i].SetActive(true);
                }
            }
                        
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