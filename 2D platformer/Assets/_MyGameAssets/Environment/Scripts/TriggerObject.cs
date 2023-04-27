using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TriggerObject : MonoBehaviour
{
    [Header("Effects:")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private float m_volume;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private ParticleSystem m_pfx;
    [SerializeField] private Light2D m_light;
    private float m_decreasingSpeedX;
    private float m_decreasingSpeedY;
    private float m_xScale;
    private float m_yScale;

    private void Start()
    {
        m_xScale = gameObject.transform.localScale.x;
        m_yScale = gameObject.transform.localScale.y;
    }

    public void DestroyObject()
    {
        gameObject.transform.DOKill();
        Destroy(gameObject);
    }

    public void ShrinkDestroy(float time)
    {
        //StartCoroutine(ShrinkDestroyC(time));
        gameObject.transform.DOScale(0, time).OnComplete(() => DestroyObject());
        //m_light.intensity = DO
    }
    
    private IEnumerator ShrinkDestroyC(float time)
    {
        m_decreasingSpeedX = gameObject.transform.localScale.x / time;
        m_decreasingSpeedY = gameObject.transform.localScale.y / time;

        while (gameObject.transform.localScale.x > 0)
        {
            m_xScale -= m_decreasingSpeedX * Time.deltaTime;
            m_yScale -= m_decreasingSpeedY * Time.deltaTime;
            gameObject.transform.localScale = new Vector3(m_xScale, m_yScale, 1);
            yield return new WaitForEndOfFrame();
        }

        // Play effects before destroying the object:



        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}