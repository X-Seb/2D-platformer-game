using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_winAudioClip;
    [Range(0, 1)][SerializeField] private float m_volume;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.GetState() == GameManager.GameState.playing &&
            PlayerPrefs.HasKey("Relic_1_Collected") &&
            PlayerPrefs.HasKey("Relic_2_Collected") &&
            PlayerPrefs.HasKey("Relic_3_Collected") &&
            PlayerPrefs.HasKey("Relic_4_Collected") &&
            PlayerPrefs.HasKey("Relic_5_Collected") &&
            PlayerPrefs.HasKey("Relic_6_Collected"))
        {
            GameTimer.instance.PlayerWon();
            m_audioSource.PlayOneShot(m_winAudioClip, m_volume);
            GameManager.instance.WinGame();
        }
    }
}
