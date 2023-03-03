using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Important:")]
    [SerializeField] private float m_spawnInterval;
    [SerializeField] private GameObject m_object;
    [SerializeField] private bool m_spawnOnAwake;
    [Tooltip("Only used for fireballs")]
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private float m_speed;
    [Header("For reference only:")]
    [SerializeField] private bool m_isSpawning;
    [Header("Effects:")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private float m_volume;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private ParticleSystem m_pfx;
    [Header("Setup")]
    [SerializeField] private Transform m_spawnTrans;

    void Start()
    {
        if (m_spawnOnAwake)
        {
            m_isSpawning = true;
            StartCoroutine(SpawnObject(m_spawnInterval));
        }
    }

    private IEnumerator SpawnObject(float time)
    {
        GameObject fireball = ObjectPooler.instance.GetObject();
        if (fireball != null)
        {
            fireball.SetActive(true);
            fireball.gameObject.transform.position = m_spawnTrans.position;
            fireball.gameObject.transform.rotation = m_spawnTrans.rotation;
            fireball.GetComponent<FireballController>().Setup(m_direction, m_speed);
            m_audioSource.PlayOneShot(m_audioClip, m_volume);
            m_pfx.Play();
        }

        yield return new WaitForSeconds(time);
        
        if (m_isSpawning)
        {
            StartCoroutine(SpawnObject(m_spawnInterval));
        }
    }
}