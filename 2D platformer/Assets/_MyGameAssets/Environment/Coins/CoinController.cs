using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private CoinManager m_coinManager;
    [Tooltip("Make sure each coinID is unique!")]
    [SerializeField] private int m_coinID;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_coinAudioSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Coin_" + m_coinID + "_Collected"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinCollected();
        }
    }

    private void CoinCollected()
    {
        m_coinManager.IncreaseCoinCount();
        Debug.Log("Coin " + m_coinID + " collected!");
        PlayerPrefs.SetInt("Coin_" + m_coinID + "_Collected", 1);
        PlayerPrefs.Save();
        m_coinAudioSource.Play();
        Destroy(gameObject);
    }
}
