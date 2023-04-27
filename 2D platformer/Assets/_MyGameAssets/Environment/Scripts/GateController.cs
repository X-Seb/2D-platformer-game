using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GateController : MonoBehaviour
{
    [Header("Info: ")]
    [SerializeField] private int m_gateID;
    [SerializeField] private CollectibleItem[] m_keys;
    [Header("Setup")]
    [SerializeField] private Collider2D m_physicalCollider;
    [SerializeField] private Collider2D m_triggerCollider;
    [Header("Effects: ")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_openAudioClip;
    [Range(0, 1)][SerializeField] float m_volume;
    [SerializeField] private Light2D m_light;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Gate_" + m_gateID + "_Unlocked"))
        {
            StartCoroutine(Unlock());
        }
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Gate_" + m_gateID + "_Unlocked"))
        {
            StartCoroutine(Unlock());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TryToUnlock();
        }
    }

    private void TryToUnlock()
    {
        // Only open the gate if you collected, but didn't use one of the keys in the array
        // TODO: fix this since it's currently using up all the keys you have
        bool keyUsed = false; ;
        for (int i = 0; i < m_keys.Length; i++)
        {
            if (!keyUsed && PlayerPrefs.HasKey("Key_" + m_keys[i].keyID + "_Collected") &&
                (!PlayerPrefs.HasKey("Key_" + m_keys[i].keyID + "_Used") || PlayerPrefs.GetInt("Key_" + m_keys[i].keyID + "_Used") == 0))
            {
                StartCoroutine(Unlock());
                PlayerPrefs.SetInt("Key_" + m_keys[i].keyID + "_Used", 1);
                keyUsed = true;
                Debug.Log("Key " + m_keys[i].keyID + " used!");
            }
        }
    }

    private IEnumerator Unlock()
    {
        // Effects: 
        m_triggerCollider.enabled = false;
        m_physicalCollider.enabled = false;
        m_spriteRenderer.color = new Color(1, 1, 1, 0.2f);
        m_audioSource.PlayOneShot(m_openAudioClip, m_volume);
        m_light.intensity = 2.0f;
        //m_pfx.Play();

        PlayerPrefs.SetInt("Gate_" + m_gateID + "_Unlocked", 1);
        yield return null;
    }
}