using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Referensi UI")]
    public Slider sliderMusik;
    public Slider sliderSensitivitas;

    private void Start()
    {
        // 1. Memuat data yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("VolumeMusik", 1f);
        float savedSensitivitas = PlayerPrefs.GetFloat("SensitivitasKamera", 1f);

        // 2. Menerapkan nilai ke tampilan Slider UI
        if (sliderMusik != null) sliderMusik.value = savedVolume;
        if (sliderSensitivitas != null) sliderSensitivitas.value = savedSensitivitas;

        // 3. Terapkan ke dalam game
        SetVolumeMusik(savedVolume);
        SetSensitivitas(savedSensitivitas);
    }

    public void SetVolumeMusik(float volume)
    {
        // Langsung panggil musik dari instance BGMManager (Sangat aman meski bolak-balik scene!)
        if (BGMManager.instance != null && BGMManager.instance.audioSource != null)
        {
            BGMManager.instance.audioSource.volume = volume;
        }
        
        PlayerPrefs.SetFloat("VolumeMusik", volume);
        PlayerPrefs.Save();
    }

    public void SetSensitivitas(float sensitivitas)
    {
        PlayerPrefs.SetFloat("SensitivitasKamera", sensitivitas);
        PlayerPrefs.Save();
    }
}