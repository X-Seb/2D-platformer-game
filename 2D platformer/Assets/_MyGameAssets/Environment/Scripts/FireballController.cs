using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireballController : MonoBehaviour
{
    [Header("Important:")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_volume;
    [Header("Other stuff:")]
    [SerializeField] private Light2D m_light;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private CircleCollider2D m_collider;
    [Header("For reference only:")]
    [SerializeField] private bool m_isMoving;
    [SerializeField] private Vector3 m_direction;
    [SerializeField] private Vector3 m_displacement;
    
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
            StartCoroutine(Collided());
        }
    }

    public void Setup(Vector3 direction, float speed)
    {
        this.m_direction = direction;
        this.m_speed = speed;

        // Reset the fireball to it's original state
        this.m_isMoving = true;
        gameObject.SetActive(true);
        m_collider.enabled = true;
        m_light.intensity = 1.0f;
        m_spriteRenderer.color = new Color(255, 255, 255, 255);
    }


    private IEnumerator Collided()
    {
        Debug.Log("Fireball collided!");
        m_isMoving = false;
        m_collider.enabled = false;
        m_audioSource.PlayOneShot(m_audioClip, m_volume);
        m_light.intensity = 0.0f;
        m_spriteRenderer.color = new Color(0, 0, 0, 0);
        m_pfx.Play();

        // Wait before returning it to the pool of objects
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
