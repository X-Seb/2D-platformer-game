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
    private string m_text = "";

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
        if (GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_timeElapsed += Time.deltaTime;
            PlayerPrefs.SetFloat("Current_Time_Elapsed", m_timeElapsed);

            if (m_isPlayerSpeedrunning)
            {
                m_text = (Mathf.Round(m_timeElapsed * 100) * 0.01f).ToString();
                m_timeElapsedText.text = m_text;
            }
        }
    }
}