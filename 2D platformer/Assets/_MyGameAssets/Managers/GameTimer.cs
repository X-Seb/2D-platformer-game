using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI elements: ")]
    [SerializeField] private GameObject m_gameTimer;
    [SerializeField] private TextMeshProUGUI m_timeElapsedText;
    [Header("Variables for reference only: ")]
    [SerializeField] private float m_timeElapsed = 0.0f;
    [SerializeField] private bool m_isPlayerSpeedrunning;
    [SerializeField] private bool m_playerWon;
    public static GameTimer instance;
    private string m_text = "";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        if (PlayerPrefs.HasKey("PlayerWon"))
        {
            m_playerWon = true;
        }
        else
        {
            m_playerWon = false;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Speedrunning") == 0)
        {
            m_isPlayerSpeedrunning = false;
            m_gameTimer.SetActive(false);
        }
        else
        {
            m_isPlayerSpeedrunning = true;
            m_gameTimer.SetActive(true);
        }

        if (PlayerPrefs.HasKey("Current_Time_Elapsed"))
        {
            m_timeElapsed = PlayerPrefs.GetFloat("Current_Time_Elapsed");
            m_text = (Mathf.Round(m_timeElapsed * 100) * 0.01f).ToString();
            m_timeElapsedText.text = m_text;
        }
        else
        {
            m_timeElapsed = 0.0f;
            m_text = (Mathf.Round(m_timeElapsed * 100) * 0.01f).ToString();
            m_timeElapsedText.text = m_text;
        }
    }

    private void Update()
    {
        if (GameManager.instance.IsPlaying() && !m_playerWon)
        {
            m_timeElapsed += Time.deltaTime;
            PlayerPrefs.SetFloat("Current_Time_Elapsed", m_timeElapsed);

            if (m_isPlayerSpeedrunning)
            {
                int roundedTime = Mathf.RoundToInt(m_timeElapsed * 100);
                float formattedTime = (float)roundedTime * 0.01f;
                m_timeElapsedText.text = formattedTime.ToString("F2");
            }
        }
    }

    public float GetTimeElapsed(bool round = false)
    {
        if (!round)
        {
            return m_timeElapsed;
        }
        else
        {
            return Mathf.RoundToInt(m_timeElapsed * 100) * 0.01f;
        }
        
    }

    public void PlayerWon()
    {
        m_playerWon = true;
    }
}