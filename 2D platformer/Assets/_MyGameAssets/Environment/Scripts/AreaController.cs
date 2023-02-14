using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    [SerializeField] private GameObject m_specialObjects;
    [SerializeField] private GameObject m_enemies;
    [SerializeField] private GameObject m_platforms;
    [SerializeField] private GameObject m_decorations;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_specialObjects.SetActive(true);
            m_enemies.SetActive(true);
            m_platforms.SetActive(true);
            m_decorations.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_specialObjects.SetActive(false);
            m_enemies.SetActive(false);
            m_platforms.SetActive(false);
            m_decorations.SetActive(false);
        }
    }
}
