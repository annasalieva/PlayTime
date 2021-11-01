using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject VolumePanel;
    public UnityEngine.UI.Toggle toggle;
    public UnityEngine.UI.Slider slider;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume", 1);
        bool mute = PlayerPrefs.GetString("mute", "no") == "no" ? false : true;
        VolumeSlider(volume);
        slider.value = volume;
        MuteToggle(mute);
        toggle.isOn = mute;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    //OPEN VOLUME CONTROL PANEL
    public void VolumePanelOpen()
    {
        pauseMenuUI.SetActive(false);
        VolumePanel.SetActive(true);
    }

    //GO BACK TO PAUSE MENU PANEL
    public void BackToPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        VolumePanel.SetActive(false);
    }

    //VOLUME SLIDER
    public void VolumeSlider(float volume)
    {
        AudioHandler.StaticAudioHandler.SetVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    //MUTE TOGGLE
    public void MuteToggle(bool state)
    {
        AudioHandler.StaticAudioHandler.Mute(state);
        if (state)
        {
            PlayerPrefs.GetString("mute", "yes");
        }
        else
        {
            PlayerPrefs.GetString("mute", "no");
        }
    }
}
