using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    private float m_decreasingSpeed;
    private bool m_isShrinking;
    private Vector3 m_zero = new Vector3(0, 0, 1);
    private float m_xScale;
    private float m_yScale;

    private void Start()
    {
        m_xScale = gameObject.transform.localScale.x;
        m_yScale = gameObject.transform.localScale.y;
    }

    private void Update()
    {
        if (gameObject.transform.localScale == m_zero)
        {
            Destroy(gameObject);
        }
        else if (m_isShrinking)
        {
            m_xScale -= m_decreasingSpeed * Time.deltaTime;
            m_yScale -= m_decreasingSpeed * Time.deltaTime;
            gameObject.transform.localScale = new Vector3(m_xScale, m_yScale, 1);
        }
    }

    public void ShrinkDestroy(float speed)
    {
        m_decreasingSpeed = speed;
        m_isShrinking = true;
    }

    public void MoveTo()
    {
        
    }

    public void MoveBack()
    {

    }
}