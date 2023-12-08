using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Emitter : MonoBehaviour
{
    [Header("Events: ")]
    [SerializeField] private UnityEvent m_activateEvent;
    [SerializeField] private UnityEvent m_disableEvent;
    [Header("Essential info: ")]
    [SerializeField] Type m_type;
    [Header("Setup: ")]
    [SerializeField] private BoxCollider2D m_collider;
    [Header("For timer only: ")]
    [SerializeField] private float m_waitTime;
    [Header("For reference only: ")]
    [SerializeField] private bool m_isActivated;
    [SerializeField] private bool m_justActivated;
    [SerializeField] private bool m_isWaiting;

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

    private void OnEnable()
    {
        TryToActivate();
    }

    private void Start()
    {
        TryToActivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") /*&& GameManager.instance.IsPlaying() */)
        {
            if (m_type == Type.forever && !m_isActivated)
            {
                Activate();
            }
            else if (m_type == Type.onOff)
            {
                if (m_isActivated && !m_justActivated)
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
        m_activateEvent.Invoke();
        m_isActivated = true;
        PlayerPrefs.SetInt(gameObject.name + "_Activated", 1);
    }

    private void Deactivate()
    {
        m_disableEvent.Invoke();
        m_isActivated = false;
        PlayerPrefs.SetInt(gameObject.name + "_Activated", 0);
    }

    private void TryToActivate()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "_Activated") && PlayerPrefs.GetInt(gameObject.name + "_Activated") == 1)
        {
            m_isActivated = true;
            m_activateEvent.Invoke();
        }
        else
        {
            m_isActivated = false;
        }
    }

    private IEnumerator StartDelay()
    {
        m_justActivated = true;
        yield return new WaitForSeconds(0.5f);
        m_justActivated = false;
    }

    private IEnumerator StartTimer()
    {
        m_isWaiting = true;
        yield return new WaitForSeconds(m_waitTime);
        m_isWaiting = false;
        Deactivate();
    }
}