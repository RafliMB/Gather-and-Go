using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI teksInformasiKunci;

    private GateController gerbang;

    private void Start()
    {
        gerbang = FindAnyObjectByType<GateController>();
        UpdateUI();
    }

public void UpdateUI() 
    {
        if (gerbang != null)
        {
            teksInformasiKunci.text = "Kunci: " + gerbang.kunciTerkumpul + " / " + gerbang.jumlahKunciDibutuhkan;
            
            if (gerbang.kunciTerkumpul >= gerbang.jumlahKunciDibutuhkan)
            {
                teksInformasiKunci.color = Color.green; 
            }
        }
    }
}