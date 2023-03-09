using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    public static RelicManager instance;
    [Header("Important relic info: ")]
    [SerializeField] private int m_relicCount;
    [SerializeField] public static bool s_showRelics = false;
    [Header("Objects to determine min/max position:")]
    [SerializeField] private GameObject m_topPos;
    [SerializeField] private GameObject m_bottonPos;
    [Header("Variables for moving: ")]
    [SerializeField] private float m_speed;
    [SerializeField] private Vector3 m_targetPos = new Vector3(0, 0, 0);
    [SerializeField] private float m_yPos;
    [Header("UI elements: ")]
    [SerializeField] private RectTransform m_rectTransform;
    [SerializeField] private Image m_relic1Image;
    [SerializeField] private Image m_relic2Image;
    [SerializeField] private Image m_relic3Image;
    [SerializeField] private Image m_relic4Image;
    [SerializeField] private Image m_relic5Image;
    [SerializeField] private Image m_relic6Image;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateRelicColors();
        s_showRelics = false; // This will become true instantly if the player is spawned in a checkpoint box
    }

    private void Update()
    {
        if (s_showRelics && m_rectTransform.position.y < m_topPos.transform.position.y)
        {
            m_yPos += Time.deltaTime * m_speed;
            m_targetPos = new Vector3(m_rectTransform.position.x, m_yPos, m_rectTransform.position.z);
            m_rectTransform.position = m_targetPos;
        }
        else if (!s_showRelics && m_rectTransform.position.y > m_bottonPos.transform.position.y)
        {
            m_yPos -= Time.deltaTime * m_speed;
            m_targetPos = new Vector3(m_rectTransform.position.x, m_yPos, m_rectTransform.position.z);
            m_rectTransform.position = m_targetPos;
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
        StartCoroutine(ShowRelics());
    }

    private IEnumerator ShowRelics()
    {
        s_showRelics = true;
        yield return new WaitForSeconds(2);
        s_showRelics = false;
    }
}