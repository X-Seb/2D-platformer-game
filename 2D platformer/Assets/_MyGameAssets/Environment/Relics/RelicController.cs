using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private RelicManager m_relicManager;
    [Tooltip("Make sure each relicID is unique1")]
    [SerializeField] private int m_relicID;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_relicAudioSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Relic_" + m_relicID + "_Collected"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RelicCollected();
        }
    }

    private void RelicCollected()
    {
        m_relicManager.IncreaseRelicCount();
        Debug.Log("Relic " + m_relicID + " collected!");
        PlayerPrefs.SetInt("Relic_" + m_relicID + "_Collected", 1);
        PlayerPrefs.Save();
        m_relicManager.UpdateRelicColors();
        m_relicAudioSource.Play();
        Destroy(gameObject);
    }
}