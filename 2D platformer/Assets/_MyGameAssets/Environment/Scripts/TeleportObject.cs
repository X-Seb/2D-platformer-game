using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] private TypeOfTeleportation m_type;
    [SerializeField] private GameObject m_emptyPlatform;
    [Header("Effects:")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_teleportAudioClip;
    [SerializeField] private AudioClip m_warningAudioClip;
    [SerializeField] private float m_volume;
    [Header("Automatic:")]
    [SerializeField] private float m_timePerCycle;
    [Header("For reference only")]
    [SerializeField] private Vector3 m_targetPos;
    [SerializeField] private Vector3 m_startPos;
    [SerializeField] private bool m_isAtOriginalPos = true;

    enum TypeOfTeleportation
    {
        automatic,
        whenJump,
        whenDash
    }

    private void OnEnable()
    {
        m_startPos = transform.position;
        m_targetPos = m_emptyPlatform.transform.position;
        m_isAtOriginalPos = true;

        if (m_type == TypeOfTeleportation.automatic)
        {
            StartCoroutine(Teleport());
        }
    }

    public void SetCycleTime(float time)
    {
        m_timePerCycle = time;
    }

    public void JumpTeleport()
    {
        if (m_type == TypeOfTeleportation.whenJump && gameObject.activeInHierarchy)
        {
            StartCoroutine(Teleport());
        }
    }

    public void DashTeleport()
    {
        if (m_type == TypeOfTeleportation.whenDash && gameObject.activeInHierarchy)
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        if (m_isAtOriginalPos)
        {
            // Teleport to other position
            transform.position = m_targetPos;
            m_emptyPlatform.transform.position = m_startPos;
            m_audioSource.PlayOneShot(m_teleportAudioClip, m_volume);
            m_isAtOriginalPos = false;
        }
        else
        {
            // Teleport back to inital position
            transform.position = m_startPos;
            m_emptyPlatform.transform.position = m_targetPos;
            m_audioSource.PlayOneShot(m_teleportAudioClip, m_volume);
            m_isAtOriginalPos = true;
        }

        if (m_type == TypeOfTeleportation.automatic)
        {

            // I do this since sometimes changing the cycleTime happens after the coroutine is called
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds((m_timePerCycle * 0.5f) - 0.9f);

            m_audioSource.PlayOneShot(m_warningAudioClip, 0.8f);
            yield return new WaitForSeconds(0.3f);
            m_audioSource.PlayOneShot(m_warningAudioClip, 0.8f);
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(Teleport());
        }
        else
        {
            yield return null;
        }
    }
}