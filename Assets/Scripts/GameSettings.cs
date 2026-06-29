using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TMP_Dropdown qualityDropdown, fpsDropdown;
    [SerializeField] Slider musicSlider, ambientSlider;
    private DataSettings dataSettings;


    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip cancelSound;

    void Start()
    {
        LoadSettings();
        SetUIElements();
        ApplyButton();

        Invoke("CloseSettings", 0.001f);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SettingsData"))
        {
            string data = PlayerPrefs.GetString("SettingsData");
            dataSettings = JsonUtility.FromJson<DataSettings>(data);
        }
        else
        {
            dataSettings = new DataSettings();
            SetDefaultDataValues();
        }
    }

    public void SetDefaultDataValues()
    {
        dataSettings.musicVolume = 1f;
        dataSettings.ambientVolume = 1f;
        dataSettings.fullScreen = true;
        dataSettings.quality = 1;
        dataSettings.fps = 1;

        Resolution[] resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                dataSettings.resolution = i;
                break;
            }
        }
    }

    public void SaveSettings()
    {
        string data = JsonUtility.ToJson(dataSettings);
        PlayerPrefs.SetString("SettingsData", data);
        Debug.Log("Saved! || " + data);
    }

    public void SetUIElements()
    {
        // === Sliders ===
        // Music
        musicSlider.value = dataSettings.musicVolume;

        // Ambient
        ambientSlider.value = dataSettings.ambientVolume;

        // === Dropdowns ===
        // FPS
        fpsDropdown.value = dataSettings.fps;

        // Quality
        qualityDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> optionsQuality = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            //optionsQuality[i] = new TMP_Dropdown.OptionData(QualitySettings.names[i]);
            optionsQuality.Add(new TMP_Dropdown.OptionData(QualitySettings.names[i]));
        }
        qualityDropdown.AddOptions(optionsQuality);
        qualityDropdown.value = dataSettings.quality;
    }

    public void ApplyButton()
    {
        // === Sliders ===
        // Music
        dataSettings.musicVolume = musicSlider.value;
        AudioManager.instance.SetMusicVolume(dataSettings.musicVolume);

        // Ambient
        dataSettings.ambientVolume = ambientSlider.value;
        AudioManager.instance.SetSFXVolume(dataSettings.ambientVolume);

        // === Dropdowns ===
        // FPS
        dataSettings.fps = fpsDropdown.value;
        switch (dataSettings.fps)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;
        }

        // Quality
        dataSettings.quality = qualityDropdown.value;
        QualitySettings.SetQualityLevel(dataSettings.quality);

        SaveSettings();
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void ClickButtonSound()
    {
        AudioManager.instance.PlayUISFX(clickSound);
    }
    public void CancelButtonSound()
    {
        AudioManager.instance.PlayUISFX(cancelSound);
    }
}

public class DataSettings
{
    public float musicVolume;
    public float ambientVolume;
    public bool fullScreen;
    public int fps;
    public int quality;
    public int resolution;
}