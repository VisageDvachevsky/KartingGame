using UnityEngine;

public class SettingsApplier : MonoBehaviour
{
    private const string SettingsDataKey = "SettingsData";

    private void Start()
    {
        LoadSettingsAndApply();
    }

    private void LoadSettingsAndApply()
    {
        if (PlayerPrefs.HasKey(SettingsDataKey))
        {
            string json = PlayerPrefs.GetString(SettingsDataKey);
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);

            ApplySettings(settingsData);
        }
    }

    private void ApplySettings(SettingsData settingsData)
    {
        SettingsManager settingsManager = SettingsManager.Instance;
        if (settingsManager != null)
        {
            //settingsManager.soundVolumeSlider.value = settingsData.soundVolume;
            //settingsManager.musicVolumeSlider.value = settingsData.musicVolume;
            //settingsManager.overallVolumeSlider.value = settingsData.overallVolume;
            //settingsManager.brightnessSlider.value = settingsData.brightness;

            settingsManager.UpdateSoundVolume(settingsData.soundVolume);
            settingsManager.UpdateMusicVolume(settingsData.musicVolume);
            settingsManager.UpdateMasterVolume(settingsData.overallVolume);
            settingsManager.UpdateBrightness(settingsData.brightness);
        }
    }
}
