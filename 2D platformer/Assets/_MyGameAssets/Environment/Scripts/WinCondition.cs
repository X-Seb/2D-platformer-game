using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
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
            GameManager.instance.WinGame();
        }
    }
}
