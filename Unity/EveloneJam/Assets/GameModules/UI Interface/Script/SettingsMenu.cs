using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Dropdown _graphicsLevelDropdown;

    private bool _opened = false;
    private SettingsManager _settingsManager;

    private void Start()
    {
        _settingsManager = SettingsManager.Instance;

        _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        _masterVolumeSlider.onValueChanged.AddListener(OnOverallVolumeSliderValueChanged);
        _brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderValueChanged);
        _graphicsLevelDropdown.onValueChanged.AddListener(OnGraphicsLevelChanged);

        Init();

        _opened = false;
        _container.SetActive(false);
    }

    public void Open()
    {
        if (_opened) return;

        _container.transform.localScale = Vector3.zero;
        _container.SetActive(true);
        _container.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        _opened = true;
    }

    public void Close()
    {
        if (!_opened) return;

        _container.transform.localScale = Vector3.one;
        _container.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => _container.SetActive(false)).SetUpdate(true);
        _opened = false;
    }

    private void Init()
    {
        _soundVolumeSlider.value = _settingsManager.CurrentSoundVolume;
        _musicVolumeSlider.value = _settingsManager.CurrentMusicVolume;
        _masterVolumeSlider.value = _settingsManager.CurrentMasterVolume;
        _brightnessSlider.value = _settingsManager.CurrentBrightness;
        _graphicsLevelDropdown.SetValueWithoutNotify(_settingsManager.CurrentGraphicsLevel);
    }

    private void OnGraphicsLevelChanged(int value)
    {
        _settingsManager.UpdateGraphicsLevel(value);
        _settingsManager.SaveChanges();
    }

    private void OnBrightnessSliderValueChanged(float value)
    {
        _settingsManager.UpdateBrightness(value);
        _settingsManager.SaveChanges();
    }

    private void OnOverallVolumeSliderValueChanged(float value)
    {
        _settingsManager.UpdateMasterVolume(value);
        _settingsManager.SaveChanges();
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        _settingsManager.UpdateMusicVolume(value);
        _settingsManager.SaveChanges();
    }

    private void OnSoundVolumeSliderValueChanged(float value)
    {
        _settingsManager.UpdateSoundVolume(value);
        _settingsManager.SaveChanges();
    }
}
