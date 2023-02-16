using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveGameObject : MonoBehaviour
{
    [Header("Values: ")]
    [SerializeField] private TypeOfMovement m_typeOfMovement;
    [SerializeField] private float m_timePerCycle;
    [Header("Yoyo only: ")]
    [SerializeField] private Vector3 m_displacement;
    [SerializeField] private bool m_isSmooth;
    [SerializeField] private float m_calculatedSpeed;
    [Header("Rotation only: ")]
    [SerializeField] private Transform m_rotationCenter;
    [SerializeField] private float m_calculatedRadius;
    [SerializeField] private float m_calculatedAngularVelocity;
    [Header("Teleportation only: ")]
    [SerializeField] private GameObject m_emptyPlatform;
    [SerializeField] private Vector3 m_targetPos;
    [Header("Other: ")]
    [SerializeField] private Vector3 m_startPos;
    [SerializeField] private Vector3 m_endPos;

    private enum TypeOfMovement
    {
        yoyo,
        rotation,
        teleportation
    }

    private void Start()
    {
        m_startPos = transform.position;

        if (m_typeOfMovement == TypeOfMovement.yoyo)
        {
            m_endPos = m_startPos + m_displacement;
            m_calculatedSpeed = m_displacement.magnitude / m_timePerCycle;
        }

        else if (m_typeOfMovement == TypeOfMovement.rotation)
        {
            m_calculatedAngularVelocity = 360.0f / m_timePerCycle;
        }

        else if (m_typeOfMovement == TypeOfMovement.teleportation)
        {
            m_targetPos = m_emptyPlatform.transform.position;
            StartCoroutine(Teleport());
        }
    }

    void Update()
    {
        if (m_typeOfMovement == TypeOfMovement.yoyo)
        {
            // PingPong between 0 and 1
            float time = Mathf.PingPong(Time.time * (1/ m_timePerCycle), 1);

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

        else if (m_typeOfMovement == TypeOfMovement.rotation)
        {
            transform.RotateAround(m_rotationCenter.transform.position, Vector3.up, m_calculatedAngularVelocity * Time.deltaTime);
        }
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(m_timePerCycle * 0.5f);
        Debug.Log("Teleport to empty platform.");

        transform.position = m_targetPos;
        m_emptyPlatform.transform.position = m_startPos;

        yield return new WaitForSeconds(m_timePerCycle * 0.5f);
        Debug.Log("Teleport back to starting position");

        transform.position = m_startPos;
        m_emptyPlatform.transform.position = m_targetPos;

        StartCoroutine(Teleport());
    }
}