using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PotionController : MonoBehaviour
{
    [Header("The potion: ")]
    [SerializeField] private CollectibleItem m_potion;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;
    [Range(0, 1)][SerializeField] float m_volume;
    [SerializeField] private Light2D m_light;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private BoxCollider2D m_collider;

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
            StartCoroutine(CollectedPotion());
        }
    }

    private IEnumerator CollectedPotion()
    {
        Debug.Log(m_potion.abilityName + "_Unlocked!");
        PlayerPrefs.SetInt(m_potion.abilityName + "_Unlocked", 1);
        PlayerPrefs.Save();
        GameManager.instance.CollectItem(m_potion);
        PlayerManager.instance.UpdatePlayerPowers();

        // Do the effects: 
        m_audioSource.PlayOneShot(m_audioClip, m_volume);
        m_light.intensity = 0.0f;
        m_spriteRenderer.color = new Color(0, 0, 0, 0);
        m_pfx.Play();

        // Wait before destroying the relic:
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
