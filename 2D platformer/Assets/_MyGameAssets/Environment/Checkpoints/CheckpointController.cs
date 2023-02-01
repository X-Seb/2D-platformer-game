using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Each checkpointID has to be unique: ")]
    [SerializeField] private int m_checkpointID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Checkpoint_" + m_checkpointID + "_Unlocked", 1);
            PlayerPrefs.SetInt("Last_Checkpoint", m_checkpointID);
            RelicManager.s_showRelics = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RelicManager.s_showRelics = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RelicManager.s_showRelics = false;
        }
    }
}