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
    [SerializeField] private bool m_clockwise;
    [SerializeField] private Transform m_rotationCenter;
    [SerializeField] private Vector3 m_rotationAxis;
    [SerializeField] private float m_calculatedAngularVelocity;
    [Header("One time only:")]
    [SerializeField] private Vector3 m_displacement1;
    [SerializeField] private int m_direction;
    [Header("Other: ")]
    [SerializeField] private Vector3 m_startPos;
    [SerializeField] private Vector3 m_endPos;
    
    private enum TypeOfMovement
    {
        yoyo,
        rotation,
        onCommand
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

            if (m_clockwise)
            {
                m_direction = -1;
            }
            else
            {
                m_direction = 1;
            }
        }
        else if (m_typeOfMovement == TypeOfMovement.onCommand)
        {

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
            transform.RotateAround(m_rotationCenter.transform.position, m_rotationAxis, m_calculatedAngularVelocity * Time.deltaTime * m_direction);
            transform.rotation = Quaternion.identity;
        }
    }
}