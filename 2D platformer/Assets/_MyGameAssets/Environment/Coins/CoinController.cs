using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CoinController : MonoBehaviour
{
    [Header("Each coinID must be unique! ")]
    [SerializeField] private int coinID;
    [Header("Setup: ")]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private GameObject m_spriteRenderer;
    [SerializeField] private Light2D m_light2D;
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
        if (collision.CompareTag("Player") && GameManager.instance.IsPlaying() && !m_isCollected)
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
        m_light2D.intensity = 0.0f;

        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}