using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private RelicManager m_relicManager;
    [Tooltip("The relic: ")]
    [SerializeField] private CollectibleItem m_relic;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_itemAudioSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Relic_" + m_relic.relicID + "_Collected"))
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
        Debug.Log("Relic " + m_relic.relicID + " collected!");
        PlayerPrefs.SetInt("Relic_" + m_relic.relicID + "_Collected", 1);
        PlayerPrefs.Save();

        GameManager.instance.CollectItem(m_relic);
        m_relicManager.UpdateRelicColors();
        m_itemAudioSource.Play();
        Destroy(gameObject);
    }
}