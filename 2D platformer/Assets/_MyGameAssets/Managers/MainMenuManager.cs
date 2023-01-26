using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI: ")]
    [SerializeField] private GameObject m_mainScreen;
    [SerializeField] private GameObject m_settingsScreen;
    [SerializeField] private GameObject m_creditsScreen;
    [Header("Audio: ")]
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private Slider m_volumeSlider;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_buttonSound;
    [Range(0, 1.0f)][SerializeField] private float m_soundEffectsVolume = 1.0f;
    [Header("Quality: ")]
    [SerializeField] private TMP_Dropdown m_qualityDropdown;
    [Header("Difficulty: ")]
    [SerializeField] private TMP_Dropdown m_difficultyDropdown;

    void Start()
    {
        m_mainScreen.SetActive(true);
        m_settingsScreen.SetActive(false);
        m_creditsScreen.SetActive(false);

        SetSpeedrunningMode(false);

        if (PlayerPrefs.HasKey("Volume"))
        {
            m_audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
            m_volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 0.0f);
        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
            m_qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }

        if (PlayerPrefs.HasKey("Difficulty"))
        {
            m_difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty");
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
    }

    public void CreditsButton()
    {
        m_mainScreen.SetActive(false);
        m_creditsScreen.SetActive(true);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void SettingsButton()
    {
        m_mainScreen.SetActive(false);
        m_settingsScreen.SetActive(true);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void BackButton()
    {
        m_mainScreen.SetActive(true);
        m_settingsScreen.SetActive(false);
        m_creditsScreen.SetActive(false);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void SetVolume(float volume)
    {
        m_audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void QuitApp()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void SetQualityLevel(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetDifficultyLevel(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void SetSpeedrunningMode(bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt("Speedrunning", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Speedrunning", 0);
        }
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }
}