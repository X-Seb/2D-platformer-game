using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    [Header("Important: ")]
    [SerializeField] private int m_relicCount;
    [SerializeField] private bool m_showRelics = false;
    [SerializeField] private GameObject m_topPos;
    [SerializeField] private GameObject m_bottonPos;
    [SerializeField] private Vector3 m_targetPos = new Vector3(0, 0, 0);
    [Header("UI elements: ")]
    [SerializeField] private GameObject m_relicBackground;
    [SerializeField] private Image m_relic1Image;
    [SerializeField] private Image m_relic2Image;
    [SerializeField] private Image m_relic3Image;
    [SerializeField] private Image m_relic4Image;
    [SerializeField] private Image m_relic5Image;
    [SerializeField] private Image m_relic6Image;

    private void Start()
    {
        UpdateRelicColors();
    }

    private void Update()
    {
        if (m_showRelics && m_relicBackground.transform.position.y < m_topPos.transform.position.y)
        {
            m_targetPos = new Vector3();
            m_relicBackground.transform.position = m_targetPos;
        }
        else if (!m_showRelics && m_relicBackground.transform.position.y > m_bottonPos.transform.position.y)
        {
            m_relicBackground.transform.position = m_targetPos;
        }
    }

    public void UpdateRelicColors()
    {
        if (PlayerPrefs.HasKey("Relic_" + 1 + "_Collected"))
        {
            m_relic1Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic1Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 2 + "_Collected"))
        {
            m_relic2Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic2Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 3 + "_Collected"))
        {
            m_relic3Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic3Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 4 + "_Collected"))
        {
            m_relic4Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic4Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 5 + "_Collected"))
        {
            m_relic5Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic5Image.color = new Color32(0, 0, 0, 255);
        }

        if (PlayerPrefs.HasKey("Relic_" + 6 + "_Collected"))
        {
            m_relic6Image.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            m_relic6Image.color = new Color32(0, 0, 0, 255);
        }
    }

    public void IncreaseRelicCount()
    {
        m_relicCount++;
        PlayerPrefs.SetInt("Relic_Count", m_relicCount);

    }
}