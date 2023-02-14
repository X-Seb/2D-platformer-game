using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsManager : MonoBehaviour
{
    [SerializeField] private string[] m_tagNames;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < m_tagNames.Length; i++)
        {
            if (collision.gameObject.CompareTag(m_tagNames[i]) && gameObject.isStatic)
            {
                collision.gameObject.SetActive(true);
                Debug.Log("Just activated " + gameObject.name);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < m_tagNames.Length; i++)
        {
            if (collision.gameObject.CompareTag(m_tagNames[i]) && gameObject.isStatic)
            {
                collision.gameObject.SetActive(false);
                Debug.Log("Just deactivated " + gameObject.name);
            }
        }
    }
}