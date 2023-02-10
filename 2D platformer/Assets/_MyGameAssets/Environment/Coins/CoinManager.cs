using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_coinCountText;
    [SerializeField] private int m_coinCount;

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
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("Coin_Count", m_coinCount);
    }
}