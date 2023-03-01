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
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private Vector3 m_displacement;

    private void Start()
    {
        m_isMoving = true;
        //float angleX = Mathf.Sin(transform.localRotation.z);
        //float angleY = Mathf.Cos(transform.localRotation.z);
        //m_direction = new Vector3(0, 1, 0);
    }

    void Update()
    {
        if (m_isMoving)
        {
            m_displacement = m_direction * m_speed * Time.deltaTime;
            transform.position += m_displacement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Tilemap Ground") ||
            collision.gameObject.CompareTag("Platform") ||
            (collision.gameObject.CompareTag("Ground") && collision.gameObject.GetComponent<ObjectSpawner>() == null))
        {
            Collided();
        }
    }

    public void Setup(Vector3 direction, float speed)
    {
        this.m_direction = direction;
        this.m_speed = speed;
    }

    private IEnumerator Collided()
    {
        Debug.Log("Fireball collided!");
        m_audioSource.PlayOneShot(m_audioClip, m_volume);
        m_pfx.Play();
        m_light.intensity = 0.0f;
        m_isMoving = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
