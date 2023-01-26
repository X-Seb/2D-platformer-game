using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timeElapsedText;
    [SerializeField] private float m_timeElapsed = 0.0f;
    private string m_text = "";

    private void Start()
    {
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
            m_text = (Mathf.Round(m_timeElapsed * 100) * 0.01f).ToString();
            m_timeElapsedText.text = m_text;
            PlayerPrefs.SetFloat("Current_Time_Elapsed", m_timeElapsed);
        }
    }
}
