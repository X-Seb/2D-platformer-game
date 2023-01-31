using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [Header("UI stuff: ")]
    [SerializeField] private GameObject m_loadingScreen;
    [Header("Checkpoint stuff: ")]
    [SerializeField] private int m_lastCheckpoint;
    [Header("Main menu: ")]
    [SerializeField] private MainMenuManager m_mainMenuManager;
    [Header("Levels: ")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private GameObject m_player;

    private void Awake()
    {
        // Makes this class a singleton
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

    private void TryToGetVariables()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            m_mainMenuManager = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            m_player = GameObject.Find("Player");
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {

        }
    }

    public void LoadScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadPlayerAtCheckpoint(sceneBuildIndex));
    }

    private IEnumerator LoadPlayerAtCheckpoint(int sceneBuildIndex)
    {
        //Fade in the loading screen + disable other screens
        TryToGetVariables();
        m_loadingScreen.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            m_mainMenuManager.LeavingScene();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            m_gameManager.LeavingScene();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {

        }

        //Wait before loading the scene
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
        TryToGetVariables();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //Move the player to the correct checkpoint
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {

        }

        //Fade out the loading Screen
        m_loadingScreen.SetActive(false);
    }
}