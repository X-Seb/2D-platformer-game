using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GateController : MonoBehaviour
{
    [Header("Info: ")]
    [SerializeField] private CollectibleItem m_key;
    [Header("Setup")]
    [SerializeField] private Collider2D m_physicalCollider;
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_audioSource;
    [Range(0, 1)][SerializeField] float m_volume;
    [SerializeField] private Light2D m_light;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private BoxCollider2D m_collider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Key_" + m_key.keyID + "_Collected"))
        {
            StartCoroutine(Unlock());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        yield return null;
    }
}