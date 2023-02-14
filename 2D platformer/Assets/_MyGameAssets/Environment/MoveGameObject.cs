using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObject : MonoBehaviour
{
    [Header("Values: (only change speed and displacement)")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector3 m_displacement;
    [SerializeField] private bool m_isSmooth;
    [SerializeField] private Vector3 m_startPos;
    [SerializeField] private Vector3 m_endPos;

    private void Start()
    {
        m_startPos = transform.position;
        m_endPos = m_startPos + m_displacement;
    }

    void Update()
    {
        //PingPong between 0 and 1
        float time = Mathf.PingPong(Time.time * speed, 1);

        if (m_isSmooth)
        {
            transform.position = new Vector3(Mathf.SmoothStep(m_startPos.x, m_endPos.x, time),
                Mathf.SmoothStep(m_startPos.y, m_endPos.y, time), 0);
        }
        else
        {
            transform.position = Vector3.Lerp(m_startPos, m_endPos, time);
        }
    }
}