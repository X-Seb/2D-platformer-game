using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireballController : MonoBehaviour
{
    [Header("Important:")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_volume;
    [Header("Effects:")]
    [SerializeField] private Light2D m_light;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private ParticleSystem m_pfx;
    [Header("For reference only:")]
    [SerializeField] private bool m_isMoving;
    [SerializeField] private Vector3 m_displacement;

    void Update()
    {
        if (m_isMoving)
        {
            m_displacement = new Vector3(0, 1, 0) * m_speed * Time.deltaTime;
            transform.position += m_displacement;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || (collision.CompareTag("Ground") && collision.GetComponent<ObjectSpawner>() == null))
        {
            Collided();
        }
    }

    private IEnumerator Collided()
    {
        m_audioSource.PlayOneShot(m_audioClip, m_volume);
        m_pfx.Play();
        m_light.intensity = 0.0f;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
