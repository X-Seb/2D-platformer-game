using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    [Header("Each AreaID must be unique:")]
    [SerializeField] private int m_areaID;
    [Header("Setup:")]
    [SerializeField] private GameObject m_specialObjects;
    [SerializeField] private GameObject m_enemies;
    [SerializeField] private GameObject m_platforms;
    [SerializeField] private GameObject m_decorations;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Area_" + m_areaID + "_Active"))
        {
            PlayerPrefs.SetInt("Area_" + m_areaID + "_Active", 0);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Area_" + m_areaID + "_Active") == 1 ||
            PlayerPrefs.GetString("Last_Checkpoint").Substring(0,1) == m_areaID.ToString())
        {
            m_specialObjects.SetActive(true);
            m_enemies.SetActive(true);
            m_platforms.SetActive(true);
            m_decorations.SetActive(true);
        }
        else
        {
            m_specialObjects.SetActive(false);
            m_enemies.SetActive(false);
            m_platforms.SetActive(false);
            m_decorations.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Area_" + m_areaID + "_Active", 1);
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
            PlayerPrefs.SetInt("Area_" + m_areaID + "_Active", 0);
            m_specialObjects.SetActive(false);
            m_enemies.SetActive(false);
            m_platforms.SetActive(false);
            m_decorations.SetActive(false);
        }
    }
}