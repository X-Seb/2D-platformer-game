using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.Events;

public class Receiver : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private string m_key;
    [Header("Events: ")]
    [SerializeField] private UnityEvent m_activateEvent;
    [SerializeField] private UnityEvent m_unactiveEvent;

    private void Awake()
    {
        if (m_activateEvent == null)
        {
            m_activateEvent = new UnityEvent();
        }
        if (m_unactiveEvent == null)
        {
            m_unactiveEvent = new UnityEvent();
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(m_key) && PlayerPrefs.GetInt(m_key) == 1)
        {
            m_activateEvent.Invoke();
        }
        else
        {
            m_unactiveEvent.Invoke();
        }
    }

    public void DestroyObject()
    {
        gameObject.transform.DOKill();
        Destroy(gameObject);
    }
}