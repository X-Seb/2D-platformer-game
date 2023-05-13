using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    [Header("Info: ")]
    [SerializeField] private int m_gateID;
    [SerializeField] private CollectibleItem[] m_requiredObjects;
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
    private bool m_isUnlocked;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Gate_" + m_gateID + "_Unlocked"))
        {
            m_isUnlocked = true;
            StartCoroutine(Unlock());
        }
        else
        {
            m_isUnlocked = false;
        }
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Gate_" + m_gateID + "_Unlocked"))
        {
            m_isUnlocked = true;
            StartCoroutine(Unlock());
        }
        else
        {
            m_isUnlocked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.GetState() == GameManager.GameState.playing && !m_isUnlocked)
        {
            TryToUnlock();
        }
    }

    private void TryToUnlock()
    {
        // Only open the gate if you collected, but didn't use one of the keys in the array
        bool keyUsed = false;

        foreach (CollectibleItem key in m_requiredObjects)
        {
            if (!keyUsed && PlayerPrefs.HasKey("Key_" + key.keyID + "_Collected") &&
                (!PlayerPrefs.HasKey("Key_" + key.keyID + "_Used") || PlayerPrefs.GetInt("Key_" + key.keyID + "_Used") == 0))
            {
                PlayerPrefs.SetInt("Key_" + key.keyID + "_Used", 1);
                keyUsed = true;
                Debug.Log("Key " + key.keyID + " used!");
                break;
            }
        }

        if (keyUsed)
        {
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        m_isUnlocked = true;
        PlayerPrefs.SetInt("Gate_" + m_gateID + "_Unlocked", 1);
        m_triggerCollider.enabled = false;
        m_physicalCollider.enabled = false;
        // Effects:
        m_spriteRenderer.color = new Color(1, 1, 1, 0.2f);
        m_audioSource.PlayOneShot(m_openAudioClip, m_volume);
        DOTween.To(() => m_light.intensity, x => m_light.intensity = x, 0.1f, 0.5f);
        m_pfx.Play();
        yield return null;
    }
}