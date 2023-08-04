using UnityEngine;
using UnityEngine.Audio;

public partial class SettingsManager : MonoBehaviour
{
    private const string SettingsDataKey = "SettingsData";
    private const string MasterAudioChannel = "Master";
    private const string SoundAudioChannel = "Sounds";
    private const string MusicAudioChannel = "Music";

    public static SettingsManager Instance { get; private set; }

    public BrightnessVisualizer brightnessVisualizer;

    public AudioMixer audioMixer;

    public float CurrentMasterVolume { get; private set; } = 0f;
    public float CurrentSoundVolume { get; private set; } = 0f;
    public float CurrentMusicVolume { get; private set; } = 0f;
    public float CurrentBrightness { get; private set; } = 0.5f;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings();
    }

    private void Start()
    {
        ApplySettings();
    }

    public void UpdateSoundVolume(float value)
    {
        audioMixer.SetFloat(SoundAudioChannel, value);
        CurrentSoundVolume = value;
    }

    public void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat(MusicAudioChannel, value);
        CurrentMusicVolume = value;
    }

    public void UpdateMasterVolume(float value)
    {
        audioMixer.SetFloat(MasterAudioChannel, value);
        CurrentMasterVolume = value;
    }

    public void UpdateBrightness(float value)
    {
        CurrentBrightness = value;
        RenderSettings.ambientIntensity = value;
    }

    public void SaveChanges()
    {
        SettingsData settingsData = new SettingsData
        {
            soundVolume = CurrentSoundVolume,
            musicVolume = CurrentMusicVolume,
            overallVolume = CurrentMasterVolume,
            brightness = CurrentBrightness
        };

        string json = JsonUtility.ToJson(settingsData);
        PlayerPrefs.SetString(SettingsDataKey, json);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(SettingsDataKey))
        {
            string json = PlayerPrefs.GetString(SettingsDataKey);
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);

            CurrentSoundVolume = settingsData.soundVolume;
            CurrentMusicVolume = settingsData.musicVolume;
            CurrentMasterVolume = settingsData.overallVolume;
            CurrentBrightness = settingsData.brightness;
        }
    }

    private void ApplySettings()
    {
        UpdateSoundVolume(CurrentSoundVolume);
        UpdateMusicVolume(CurrentMusicVolume);
        UpdateMasterVolume(CurrentMasterVolume);
        UpdateBrightness(CurrentBrightness);
    }
}
