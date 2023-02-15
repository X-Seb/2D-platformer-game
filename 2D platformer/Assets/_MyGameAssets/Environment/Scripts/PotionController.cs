using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    [Header("The potion: ")]
    [SerializeField] private CollectibleItem m_potion;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_itemAudioSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(m_potion.abilityName + "_Unlocked"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log(m_potion.abilityName + "_Unlocked!");
            PlayerPrefs.SetInt(m_potion.abilityName + "_Unlocked", 1);
            PlayerPrefs.Save();
            PlayerManager.instance.UpdatePlayerPowers();
            m_itemAudioSource.Play();
            GameManager.instance.CollectItem(m_potion);
            Destroy(gameObject);
        }
    }
}
