using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel Menu")]
    public GameObject panelPauseMenu; 

    private bool isPaused = false;

    private void Update()
    {        
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                LanjutkanGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;

        if (panelPauseMenu != null)
        {
            panelPauseMenu.SetActive(true); 
        }
        
        Time.timeScale = 0f; 
    }

    
    public void LanjutkanGame()
    {
        isPaused = false;

        if (panelPauseMenu != null)
        {
            panelPauseMenu.SetActive(false); 
        }

        Time.timeScale = 1f; 
    }
}