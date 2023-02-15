using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Setup: ")]
    [Tooltip("Make sure the z component of the offset vector is in front of your other 2D objects.")]
    [SerializeField] Vector3 m_offset = Vector3.zero;
    [SerializeField] GameObject m_player;

    private void LateUpdate()
    {
        if (GameManager.instance.GetState() != GameManager.GameState.lose)
        {
            transform.position = m_player.transform.position + m_offset;
        }
    }
}
