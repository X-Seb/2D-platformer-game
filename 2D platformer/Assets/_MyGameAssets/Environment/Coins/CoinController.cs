using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private ParticleSystem m_pfx;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Coin_" + gameObject.GetHashCode() + "_Collected"))
        {
            Destroy(base.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            CoinCollected();
        }
    }

    private IEnumerator CoinCollected()
    {
        CoinManager.instance.IncreaseCoinCount();
        Debug.Log("Coin " + gameObject.GetHashCode() + " collected!");
        PlayerPrefs.SetInt("Coin_" + gameObject.GetHashCode() + "_Collected", 1);
        PlayerPrefs.Save();
        m_pfx.Play();
        m_collider.enabled = false;
        yield return new WaitForSeconds(5.0f);
        Destroy(base.gameObject);
    }
}
