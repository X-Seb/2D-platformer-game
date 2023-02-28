using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectDifficultyController : MonoBehaviour
{
    [Header("The game object will be destroyed if you're playing at a different difficulty.")]
    [SerializeField] private Difficulty m_difficulty;
    //[Header("The speed of the object can also be changed.")]
    //[SerializeField] private bool m_changeSpeed;
    //[SerializeField] 

    private enum Difficulty
    {
        all,
        mediumOrHard,
        easyOrMedium,
        easy,
        medium,
        hard
    }

    private void Awake()
    {
        // Reminder: 2 is easy, 1 is medium, 0 is hard)
        // Destroy the game object if the current difficulty doesn't match the game objects difficulty
        if (PlayerPrefs.GetInt("Difficulty") == 0 &&
            (m_difficulty == Difficulty.easyOrMedium ||
            m_difficulty == Difficulty.easy ||
            m_difficulty == Difficulty.medium))
        {
            Destroy(gameObject);
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 1 &&
            (m_difficulty == Difficulty.easy ||
            m_difficulty == Difficulty.hard))
        {
            Destroy(gameObject);
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 2 &&
            (m_difficulty == Difficulty.mediumOrHard ||
            m_difficulty == Difficulty.medium ||
            m_difficulty == Difficulty.hard))
        {
            Destroy(gameObject);
        }
    }
}