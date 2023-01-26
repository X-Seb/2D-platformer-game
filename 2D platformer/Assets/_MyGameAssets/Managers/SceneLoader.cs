using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [SerializeField] private int m_lastCheckpoint;



    private void Awake()
    {
        //Makes this class a singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevelLastCheckpoint()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Go back to Main Menu");
    }

    public void LoadRewardScene()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Go to the final scene for a reward!");
    }
}
