using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_coinCountText;
    [SerializeField] private static int m_coinCount;
    public static CoinManager instance;
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_coinAudioSource;
    [SerializeField] private AudioClip m_collectedSFX;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Coin_Count"))
        {
            m_coinCount = PlayerPrefs.GetInt("Coin_Count");
        }
        else
        {
            PlayerPrefs.SetInt("Coin_Count", 0);
            m_coinCount = 0;
        }
        m_coinCountText.text = m_coinCount.ToString();
    }

    public void IncreaseCoinCount()
    {
        m_coinCount++;
        m_coinCountText.text = m_coinCount.ToString();
        PlayerPrefs.SetInt("Coin_Count", m_coinCount);
        m_coinAudioSource.PlayOneShot(m_collectedSFX);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("Coin_Count", m_coinCount);
    }
}