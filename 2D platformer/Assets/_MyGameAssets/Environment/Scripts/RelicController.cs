using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RelicController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private RelicManager m_relicManager;
    [SerializeField] private CollectibleItem m_relic;
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;
    [Range(0, 1)][SerializeField] float m_volume;
    [SerializeField] private Light2D m_light;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private BoxCollider2D m_collider;

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
            StartCoroutine(RelicCollected());
        }
    }

    private IEnumerator RelicCollected()
    {
        // Update essential game info:
        m_relicManager.IncreaseRelicCount();
        Debug.Log("Relic " + m_relic.relicID + " collected!");
        PlayerPrefs.SetInt("Relic_" + m_relic.relicID + "_Collected", 1);
        PlayerPrefs.Save();
        m_relicManager.UpdateRelicColors();
        m_collider.enabled = false;

        // Go to the collected item screen: 
        GameManager.instance.CollectItem(m_relic);

        // Do the effects: 
        m_audioSource.PlayOneShot(m_audioClip, m_volume);
        m_light.intensity = 0.0f;
        m_spriteRenderer.color = new Color(0, 0, 0, 0);
        m_pfx.Play();

        // Wait before destroy the relic: 
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}