using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpecialItemController : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private CollectibleItem m_item;
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_audioSource;
    [Range(0, 1)][SerializeField] float m_volume;
    [SerializeField] private Light2D m_light;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private BoxCollider2D m_collider;

    private void Awake()
    {
        if (m_item.itemType == CollectibleItem.itemCategory.relic && PlayerPrefs.HasKey("Relic_" + m_item.relicID + "_Collected"))
        {
            Destroy(gameObject);
        }
        else if (m_item.itemType == CollectibleItem.itemCategory.potion && PlayerPrefs.HasKey(m_item.abilityName + "_Unlocked"))
        {
            Destroy(gameObject);
        }
        else if (m_item.itemType == CollectibleItem.itemCategory.key && PlayerPrefs.HasKey("Key_" + m_item.keyID + "_Collected"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(ItemCollected());
        }
    }

    private IEnumerator ItemCollected()
    {
        // Update essential game info based on what type of object it is:
        if (m_item.itemType == CollectibleItem.itemCategory.relic)
        {
            RelicManager.instance.IncreaseRelicCount();
            PlayerPrefs.SetInt("Relic_" + m_item.relicID + "_Collected", 1);
            RelicManager.instance.UpdateRelicColors();
            Debug.Log("Relic " + m_item.relicID + " collected!");
        }
        else if (m_item.itemType == CollectibleItem.itemCategory.potion)
        {
            PlayerPrefs.SetInt(m_item.abilityName + "_Unlocked", 1);
            PlayerManager.instance.UpdatePlayerPowers();
            Debug.Log(m_item.abilityName + "_Unlocked!");
        }
        else if (m_item.itemType == CollectibleItem.itemCategory.key)
        {
            PlayerPrefs.SetInt("Key_" + m_item.keyID + "_Collected", 1);
            Debug.Log("Key " + m_item.keyID + "Collected!");
        }

        PlayerPrefs.Save();
        m_collider.enabled = false;

        // Go to the collected item screen: 
        GameManager.instance.CollectItem(m_item);

        // Do the effects: 
        m_audioSource.PlayOneShot(m_item.collectedSFX, m_volume);
        StartCoroutine(GameManager.instance.FadeAudio(m_audioSource, 5, 0));
        m_light.intensity = 0.0f;
        m_spriteRenderer.color = new Color(255, 255, 255, 0);
        m_pfx.Play();

        // Wait before destroying the item:
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}