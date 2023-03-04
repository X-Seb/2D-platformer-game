using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectDifficultyController : MonoBehaviour
{
    [Header("The game object will be destroyed if you're playing at a different difficulty.")]
    [SerializeField] private Difficulty m_difficulty;
    [Header("For cycle time of moving/teleporting objects can also be changed")]
    [SerializeField] private bool m_changeCycleTime;
    [SerializeField] private float m_easyCycleTime;
    [SerializeField] private float m_mediumCycleTime;
    [SerializeField] private float m_hardCycleTime;
    private int difficulty;
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
        difficulty = PlayerPrefs.GetInt("Difficulty");
        // Reminder: 2 is easy, 1 is medium, 0 is hard)
        // Destroy the game object if the current difficulty doesn't match the game objects difficulty
        if (difficulty == 0 &&
            (m_difficulty == Difficulty.easyOrMedium ||
            m_difficulty == Difficulty.easy ||
            m_difficulty == Difficulty.medium))
        {
            Destroy(gameObject);
        }
        else if (difficulty == 1 &&
            (m_difficulty == Difficulty.easy ||
            m_difficulty == Difficulty.hard))
        {
            Destroy(gameObject);
        }
        else if (difficulty == 2 &&
            (m_difficulty == Difficulty.mediumOrHard ||
            m_difficulty == Difficulty.medium ||
            m_difficulty == Difficulty.hard))
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (!m_changeCycleTime)
        {
            return;
        }
        else if (gameObject.GetComponent<MoveGameObject>() != null)
        {
            MoveGameObject moveGameObject = gameObject.GetComponent<MoveGameObject>();

            if (difficulty == 0)
            {
                moveGameObject.SetCycleTime(m_hardCycleTime);
            }
            else if (difficulty == 1)
            {
                moveGameObject.SetCycleTime(m_mediumCycleTime);
            }
            else if (difficulty == 2)
            {
                moveGameObject.SetCycleTime(m_easyCycleTime);
            }
        }
        else if (gameObject.GetComponent<TeleportObject>() != null)
        {
            TeleportObject teleportObject = gameObject.GetComponent<TeleportObject>();

            if (difficulty == 0)
            {
                teleportObject.SetCycleTime(m_hardCycleTime);
            }
            else if (difficulty == 1)
            {
                teleportObject.SetCycleTime(m_mediumCycleTime);
            }
            else if (difficulty == 2)
            {
                teleportObject.SetCycleTime(m_easyCycleTime);
            }
        }
    }
}