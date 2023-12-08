using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawning: ")]
    [SerializeField] private float m_spawnInterval;
    [SerializeField] private bool m_randomizeSpawnInterval;
    [SerializeField] private float m_minSpawnTime;
    [SerializeField] private float m_maxSpawnTime;
    [Header("Object speed: ")]
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_randomizeSpeed;
    [SerializeField] private float m_minSpeedTime;
    [SerializeField] private float m_maxSpeedTime;
    [Header("Other: ")]
    [SerializeField] private bool m_spawnOnAwake;
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private Quaternion m_rotation;
    [Header("For reference only:")]
    [SerializeField] private bool m_isSpawning = false;
    [Header("Effects:")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private float m_volume;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private ParticleSystem m_pfx;
    [Header("Setup")]
    [SerializeField] private Transform m_spawnTrans;
    private bool m_isReady = false;

    private void Start()
    {
        m_isReady = true;

        if (m_spawnOnAwake)
        {
            m_isSpawning = true;
            StartCoroutine(SpawnObject(m_spawnInterval));
        }
    }

    private void OnEnable()
    {
        if (m_spawnOnAwake && m_isReady)
        {
            m_isSpawning = true;
            StartCoroutine(SpawnObject(m_spawnInterval));
        }
    }

    private void OnDisable()
    {
        m_isSpawning = false;
    }

    public void SetSpawnInterval(float time)
    {
        m_spawnInterval = time;
    }

    private IEnumerator SpawnObject(float time)
    {
        GameObject fireball = ObjectPooler.instance.GetObject();

        if (fireball != null)
        {

            fireball.SetActive(true);
            fireball.transform.SetPositionAndRotation(m_spawnTrans.position, m_rotation);

            if (m_randomizeSpeed)
            {
                float speed = Random.Range(m_minSpeedTime, m_maxSpeedTime);
                fireball.GetComponent<FireballController>().Setup(m_direction, speed);
            }
            else
            {
                fireball.GetComponent<FireballController>().Setup(m_direction, m_speed);
            }

            m_audioSource.PlayOneShot(m_audioClip, m_volume);
            m_pfx.Play();
        }

        yield return new WaitForSeconds(time);

        if (m_randomizeSpawnInterval && m_isSpawning)
        {
            float newTime = Random.Range(m_minSpawnTime, m_maxSpawnTime);
            StartCoroutine(SpawnObject(newTime));
        }
        else if (m_isSpawning)
        {
            StartCoroutine(SpawnObject(m_spawnInterval));
        }
    }
}