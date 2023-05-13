using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Each coinID must be unique! ")]
    [SerializeField] private int coinID;
    [Header("Setup: ")]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private GameObject m_spriteRenderer;
    private bool m_isCollected;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Coin_" + coinID + "_Collected"))
        {
            Destroy(gameObject);
        }
        else
        {
            m_isCollected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.GetState() == GameManager.GameState.playing && !m_isCollected)
        {
            m_isCollected = true;
            StartCoroutine(CoinCollected());
        }
    }

    private IEnumerator CoinCollected()
    {
        PlayerPrefs.SetInt("Coin_" + coinID + "_Collected", 1);
        PlayerPrefs.Save();
        CoinManager.instance.AdjustCoinCount(true);
        Debug.Log("Coin " + coinID + " collected!");

        // Effects: 
        m_pfx.Play();
        m_collider.enabled = false;
        m_spriteRenderer.SetActive(false);

        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}