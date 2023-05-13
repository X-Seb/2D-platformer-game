using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TriggerObject : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private AudioSource m_audioSource;
    [Range(0.0f, 1.0f)][SerializeField] private float m_volume = 0.8f;
    [SerializeField] private AudioClip m_endSFX;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private Light2D m_light;
    [SerializeField] private Collider2D m_collider;

    public void DestroyObject()
    {
        gameObject.transform.DOKill();
        Destroy(gameObject);
    }

    public void ShrinkDestroy(float time)
    {
        StartCoroutine(ShrinkDestroyC(time));
    }
    
    private IEnumerator ShrinkDestroyC(float time)
    {
        gameObject.transform.DOScale(new Vector3(0,0,0), time);
        DOTween.To(() => m_light.intensity, x => m_light.intensity = x, 0.0f, time);
        yield return new WaitForSeconds(time);

        m_collider.enabled = false;
        m_audioSource.PlayOneShot(m_endSFX, m_volume);
        m_pfx.Play();
        yield return new WaitForSeconds(5.0f);

        gameObject.transform.DOKill();
        gameObject.SetActive(false);
    }
}