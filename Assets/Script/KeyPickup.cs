using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPickup : MonoBehaviour
{
    [Header("Pengaturan Kunci")]
    public float rotasiSpeed = 100f;
    public float destroyDelay = 1.5f;

    private bool isPlayerNear = false;
    private bool sudahDiambil = false;

    private Animator playerAnim;
    private GateController gerbang;

    private void Start()
    {
        // Cari otomatis GateController di scene
        gerbang = FindAnyObjectByType<GateController>();

        if (gerbang == null)
        {
            Debug.LogError("GateController tidak ditemukan di scene!");
        }
    }

    private void Update()
    {
        // Efek rotasi kunci
        transform.Rotate(0, rotasiSpeed * Time.deltaTime, 0);

        // Jika player dekat dan tekan F
        if (isPlayerNear &&
            !sudahDiambil &&
            Keyboard.current.fKey.wasPressedThisFrame)
        {
            AmbilKunci();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            // Ambil animator player
            playerAnim = other.GetComponent<Animator>();

            Debug.Log("Tekan F untuk mengambil kunci");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerAnim = null;
        }
    }

    private void AmbilKunci()
    {
        // Mencegah spam pickup
        sudahDiambil = true;
        isPlayerNear = false;

        // Tambah jumlah kunci ke GateController
        if (gerbang != null)
        {
            gerbang.TambahKunci();

            Debug.Log("Jumlah kunci berhasil ditambahkan!");
        }
        else
        {
            Debug.LogError("GateController NULL!");
        }

        // Jalankan animasi pickup
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("pickup");
        }

        // Sembunyikan mesh kunci
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        if (mesh != null)
        {
            mesh.enabled = false;
        }

        // Matikan collider
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("Kunci berhasil diambil!");

        // Hancurkan object
        Destroy(gameObject, destroyDelay);
    }
}