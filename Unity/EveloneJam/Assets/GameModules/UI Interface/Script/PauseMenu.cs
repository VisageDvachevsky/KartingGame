using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider overallVolumeSlider;
    public Slider brightnessSlider;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
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
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);

        //SettingsManager.Instance.ApplySettingsToPauseMenu(this);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void OnSoundVolumeSliderValueChanged(float value)
    {
        SettingsManager.Instance.UpdateSoundVolume(value);
    }

    public void OnMusicVolumeSliderValueChanged(float value)
    {
        SettingsManager.Instance.UpdateMusicVolume(value);
    }

    public void OnOverallVolumeSliderValueChanged(float value)
    {
        SettingsManager.Instance.UpdateMasterVolume(value);
    }

    public void OnBrightnessSliderValueChanged(float value)
    {
        SettingsManager.Instance.UpdateBrightness(value);
    }
}
