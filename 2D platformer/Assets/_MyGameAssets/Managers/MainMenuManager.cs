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
    [SerializeField] private GameObject m_statsScreen;
    [Header("Stat screen UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_coinCountText;
    [SerializeField] private TextMeshProUGUI m_deathCountText;
    [SerializeField] private TextMeshProUGUI m_timeElapsedText;
    [SerializeField] private TextMeshProUGUI m_jumpCountText;
    [SerializeField] private TextMeshProUGUI m_dashCountText;
    [SerializeField] private TextMeshProUGUI m_airJumpCountText;
    [SerializeField] private Image m_relic1Image;
    [SerializeField] private Image m_relic2Image;
    [SerializeField] private Image m_relic3Image;
    [SerializeField] private Image m_relic4Image;
    [SerializeField] private Image m_relic5Image;
    [SerializeField] private Image m_relic6Image;
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
        m_statsScreen.SetActive(false);

        SetSettingsInformation();

        SetStatsInformation();
        UpdateRelicColors();
    }

    private void SetSettingsInformation()
    {
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

        if (!PlayerPrefs.HasKey("Speedrunning"))
        {
            SetSpeedrunningMode(false);
        }
    }

    private void SetStatsInformation()
    {
        m_coinCountText.text = PlayerPrefs.GetInt("Coin_Count").ToString();
        m_timeElapsedText.text = (Mathf.Round(PlayerPrefs.GetFloat("Current_Time_Elapsed") * 100) * 0.01).ToString();
        m_deathCountText.text = PlayerPrefs.GetInt("Death_Count").ToString();
        m_dashCountText.text = "Dashes: " + PlayerPrefs.GetInt("Dashes_Count").ToString();
        m_jumpCountText.text = "Jumps: " + PlayerPrefs.GetInt("Jumps_Count").ToString();
        m_airJumpCountText.text = "Air-jumps: " + PlayerPrefs.GetInt("AirJumps_Count").ToString();
    }

    private void UpdateRelicColors()
    {
        if (PlayerPrefs.HasKey("Relic_" + 1 + "_Collected"))
        {
            m_relic1Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic1Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 2 + "_Collected"))
        {
            m_relic2Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic2Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 3 + "_Collected"))
        {
            m_relic3Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic3Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 4 + "_Collected"))
        {
            m_relic4Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic4Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 5 + "_Collected"))
        {
            m_relic5Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic5Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 6 + "_Collected"))
        {
            m_relic6Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic6Image.color = new Color32(0, 0, 0, 255);
        }
    }

    public void PlayButton(int sceneBuildIndex)
    {
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        SceneLoader.instance.LoadScene(sceneBuildIndex);
    }

    public void CreditsButton()
    {
        m_statsScreen.SetActive(false);
        m_mainScreen.SetActive(false);
        m_creditsScreen.SetActive(true);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void SettingsButton()
    {
        m_statsScreen.SetActive(false);
        m_mainScreen.SetActive(false);
        m_settingsScreen.SetActive(true);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void StatsButton()
    {
        m_mainScreen.SetActive(false);
        m_statsScreen.SetActive(true);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void BackButton()
    {
        m_mainScreen.SetActive(true);
        m_statsScreen.SetActive(false);
        m_settingsScreen.SetActive(false);
        m_creditsScreen.SetActive(false);
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void ResetAllStats()
    {
        int quality = PlayerPrefs.GetInt("Quality");
        float volume = PlayerPrefs.GetFloat("Volume");
        int speedrunning = PlayerPrefs.GetInt("Speedrunning");
        int difficulty = PlayerPrefs.GetInt("Difficulty");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("Quality", quality);
        m_qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        PlayerPrefs.SetFloat("Volume", volume);
        m_audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
        m_volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.SetInt("Speedrunning", speedrunning);
        PlayerPrefs.SetInt("Difficulty", difficulty);
        m_difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty");
        SetStatsInformation();
        UpdateRelicColors();

        PlayerPrefs.Save();
        Debug.Log("All stats got reset!");
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

    public void LeavingScene()
    {
        m_mainScreen.SetActive(false);
        m_settingsScreen.SetActive(false);
        m_creditsScreen.SetActive(false);
        m_statsScreen.SetActive(false);
    }
}