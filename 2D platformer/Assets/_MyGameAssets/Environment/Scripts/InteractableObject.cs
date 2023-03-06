using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class InteractableObject : MonoBehaviour
{
    [Header("Events: ")]
    [SerializeField] private UnityEvent m_activateEvent;
    [SerializeField] private UnityEvent m_disableEvent;
    [Header("Set info: ")]
    [SerializeField] Type m_type;
    [Header("For timer only: ")]
    [SerializeField] private float m_waitTime;
    [Header("For reference only: ")]
    [SerializeField] private bool m_isActivated;
    [SerializeField] private bool m_isWaiting;
    [Header("Setup: ")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite m_onSprite;
    [SerializeField] private Sprite m_offSprite;
    [SerializeField] private BoxCollider2D m_collider;
    [SerializeField] private Light2D m_light;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private float m_volume;
    [SerializeField] private AudioClip m_onAudioClip;
    [SerializeField] private AudioClip m_offAudioClip;
    [SerializeField] private ParticleSystem m_pfx;

    private enum Type
    {
        forever,
        onOff,
        timer
    }

    private void Awake()
    {
        if (m_activateEvent == null)
        {
            m_activateEvent = new UnityEvent();
        }
        if (m_disableEvent == null)
        {
            m_disableEvent = new UnityEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_type == Type.forever && !m_isActivated)
            {
                Activate();
            }
            else if (m_type == Type.onOff)
            {
                if (m_isActivated)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            }
            else if (m_type == Type.timer)
            {
                if (!m_isWaiting)
                {
                    Activate();
                    StartCoroutine(StartTimer());
                }
            }
        }
    }

    private void Activate()
    {
        m_spriteRenderer.sprite = m_onSprite;
        m_audioSource.PlayOneShot(m_onAudioClip, m_volume);
        m_pfx.Play();
        m_light.color = new Color(0, 230, 0, 255);
        m_activateEvent.Invoke();
        m_isActivated = true;
    }

    private void Deactivate()
    {
        m_spriteRenderer.sprite = m_offSprite;
        m_audioSource.PlayOneShot(m_offAudioClip, m_volume);
        m_pfx.Play();
        m_light.color = new Color(230, 0, 0, 255);
        m_disableEvent.Invoke();
        m_isActivated = false;
    }

    private IEnumerator StartTimer()
    {
        m_isWaiting = true;
        yield return new WaitForSeconds(1.0f);
        m_isWaiting = false;
        Deactivate();
    }
}