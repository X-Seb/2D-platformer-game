using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemController : MonoBehaviour
{
    [Header("Either 'Dash', 'AirJump', or 'WallJump'.")]
    [SerializeField] private string m_itemAbility;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_itemAudioSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(m_itemAbility + "_Unlocked"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log(m_itemAbility + "_Unlocked!");
            PlayerPrefs.SetInt(m_itemAbility + "_Unlocked", 1);
            PlayerPrefs.Save();
            PlayerManager.instance.UpdatePlayerPowers();
            m_itemAudioSource.Play();
            GameManager.instance.CollectItem(m_itemAbility);
            Destroy(gameObject);
        }
    }
}
