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
        AdjustCoinCount();
        m_coinCountText.text = m_coinCount.ToString();
    }

    public int GetCoinCount()
    {
        if (PlayerPrefs.HasKey("Coin_Count"))
        {
            return m_coinCount;
        }
        else
        {
            return 0;
        }
    }

    public void AdjustCoinCount(bool playSFX = false)
    {
        m_coinCount = 0;
        for (int i = 1; i <= 35; i++)
        {
            if (PlayerPrefs.HasKey("Coin_" + i + "_Collected"))
            {
                m_coinCount++;
            }
        }

        m_coinCountText.text = m_coinCount.ToString();
        PlayerPrefs.SetInt("Coin_Count", m_coinCount);

        if (playSFX)
        {
            m_coinAudioSource.PlayOneShot(m_collectedSFX);
        }
    }

    private void OnDisable()
    {
        AdjustCoinCount();
        PlayerPrefs.SetInt("Coin_Count", m_coinCount);
    }
}