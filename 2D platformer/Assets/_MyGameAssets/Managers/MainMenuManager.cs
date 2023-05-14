using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    [Header("UI: ")]
    [SerializeField] private GameObject m_mainScreen;
    [SerializeField] private GameObject m_settingsScreen;
    [SerializeField] private GameObject m_creditsScreen;
    [SerializeField] private GameObject m_statsScreen;
    [SerializeField] private GameObject m_howToPlayScreen;
    [SerializeField] private CanvasGroup m_startScreenCG;
    [Header("Text UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_coinCountText;
    [SerializeField] private TextMeshProUGUI m_deathCountText;
    [SerializeField] private TextMeshProUGUI m_timeElapsedText;
    [SerializeField] private TextMeshProUGUI m_jumpCountText;
    [SerializeField] private TextMeshProUGUI m_dashCountText;
    [SerializeField] private TextMeshProUGUI m_airJumpCountText;
    [Header("Relic UI elements: ")]
    [SerializeField] private Image m_relic1Image;
    [SerializeField] private Image m_relic2Image;
    [SerializeField] private Image m_relic3Image;
    [SerializeField] private Image m_relic4Image;
    [SerializeField] private Image m_relic5Image;
    [SerializeField] private Image m_relic6Image;
    [Header("Item UI elements: ")]
    [SerializeField] private Image m_dashItemImage;
    [SerializeField] private Image m_airJumpItemImage;
    [SerializeField] private Image m_wallJumpItemImage;
    [Header("Other UI elements: ")]
    [SerializeField] private Toggle m_speedrunningToggle;
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
    [Header("Other: ")]
    [SerializeField] private bool m_areButtonsInteractable;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        SceneLoader.instance.FadeOut();
        m_mainScreen.SetActive(true);
        m_settingsScreen.SetActive(false);
        m_creditsScreen.SetActive(false);
        m_statsScreen.SetActive(false);

        SetSettingsInformation();
        SetStatsInformation();
        SetRelicColors();
        SetItemColors();
        m_areButtonsInteractable = true;
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
            m_volumeSlider.value = 0.0f;
        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
            m_qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            m_qualityDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("Difficulty"))
        {
            m_difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty");
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 1);
            m_difficultyDropdown.value = 1;
        }

        if (!PlayerPrefs.HasKey("Speedrunning"))
        {
            SetSpeedrunningMode(false);
            m_speedrunningToggle.isOn = false;
        }
        else if (PlayerPrefs.GetInt("Speedrunning") == 1)
        {
            m_speedrunningToggle.isOn = true;
        }
        else
        {
            m_speedrunningToggle.isOn = false;
        }
    }

    private void SetStatsInformation()
    {
        m_coinCountText.text = PlayerPrefs.GetInt("Coin_Count").ToString();
        m_deathCountText.text = PlayerPrefs.GetInt("Death_Count").ToString();
        m_dashCountText.text = "Dashes: " + PlayerPrefs.GetInt("Dashes_Count").ToString();
        m_jumpCountText.text = "Jumps: " + PlayerPrefs.GetInt("Jumps_Count").ToString();
        m_airJumpCountText.text = "Air-jumps: " + PlayerPrefs.GetInt("AirJumps_Count").ToString();

        // Time elapsed:
        float totalSec = PlayerPrefs.GetFloat("Current_Time_Elapsed");
        int hours = (int) (totalSec / 3600);
        int minutes = (int) ( (totalSec - (hours * 3600) )  / 60);
        int sec = (int) ( totalSec - (hours * 3600) - (minutes * 60) );
        m_timeElapsedText.text = hours + "h " + minutes + "min " + sec + "sec";
    }

    private void SetItemColors()
    {
        if (PlayerPrefs.HasKey("AirJump_Unlocked"))
        {
            m_airJumpItemImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_airJumpItemImage.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Dash_Unlocked"))
        {
            m_dashItemImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_dashItemImage.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("WallJump_Unlocked"))
        {
            m_wallJumpItemImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_wallJumpItemImage.color = new Color32(0, 0, 0, 255);
        }
    }

    private void SetRelicColors()
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
        if (m_areButtonsInteractable)
        {
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
            m_areButtonsInteractable = false;
            SceneLoader.instance.LoadScene(sceneBuildIndex);
        }
    }

    public void CreditsButton()
    {
        if (m_areButtonsInteractable)
        {
            m_statsScreen.SetActive(false);
            m_mainScreen.SetActive(false);
            m_creditsScreen.SetActive(true);
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        }
    }

    public void HowToPlayButton()
    {
        if (m_areButtonsInteractable)
        {
            m_statsScreen.SetActive(false);
            m_mainScreen.SetActive(false);
            m_howToPlayScreen.SetActive(true);
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        }
    }

    public void SettingsButton()
    {
        if (m_areButtonsInteractable)
        {
            m_statsScreen.SetActive(false);
            m_mainScreen.SetActive(false);
            m_settingsScreen.SetActive(true);
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        }
  
    }

    public void StatsButton()
    {
        if (m_areButtonsInteractable)
        {
            m_mainScreen.SetActive(false);
            m_statsScreen.SetActive(true);
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        }
    }

    public void BackButton()
    {
        if (m_areButtonsInteractable)
        {
            m_mainScreen.SetActive(true);
            m_statsScreen.SetActive(false);
            m_settingsScreen.SetActive(false);
            m_creditsScreen.SetActive(false);
            m_howToPlayScreen.SetActive(false);
            m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        }
    }

    public void PlayButtonSound()
    {
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
    }

    public void ResetAllStats()
    {
        // Conserve the player's chosen settings
        int quality = PlayerPrefs.GetInt("Quality");
        float volume = PlayerPrefs.GetFloat("Volume");
        int speedrunning = PlayerPrefs.GetInt("Speedrunning");
        int difficulty = PlayerPrefs.GetInt("Difficulty");

        PlayerPrefs.DeleteAll();

        // Reset the player's settings to the original values
        PlayerPrefs.SetInt("Quality", quality);
        m_qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        PlayerPrefs.SetFloat("Volume", volume);
        m_audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
        m_volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.SetInt("Speedrunning", speedrunning);
        PlayerPrefs.SetInt("Difficulty", difficulty);
        m_difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty");
        SetStatsInformation();
        SetRelicColors();
        SetItemColors();

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
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetDifficultyLevel(int difficulty)
    {
        m_audioSource.PlayOneShot(m_buttonSound, m_soundEffectsVolume);
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

    public void LeavingScene(float fadeTime)
    {
        StartCoroutine(LeavingSceneTransition(fadeTime));
    }

    private IEnumerator LeavingSceneTransition(float fadeTime)
    {
        m_startScreenCG.DOFade(0.0f, fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime);
        m_startScreenCG.DOKill();
        m_mainScreen.SetActive(false);
    }
}