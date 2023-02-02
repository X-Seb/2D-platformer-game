using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [Header("UI stuff: ")]
    [SerializeField] private GameObject m_loadingCanva;
    [SerializeField] private GameObject m_loadingScreen;
    [SerializeField] private TextMeshProUGUI m_funnyText;
    [Header("Checkpoint stuff: ")]
    [SerializeField] private int m_lastCheckpoint;
    [Header("Main menu: ")]
    [SerializeField] private MainMenuManager m_mainMenuManager;
    [Header("Levels: ")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private GameObject m_player;
    [SerializeField] private string[] m_funnyTextOptions;

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

    private void Start()
    {
        m_loadingCanva.SetActive(false);
        m_loadingCanva.SetActive(false);
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
        TryToGetVariables();
        StartCoroutine(LoadPlayerAtCheckpoint(sceneBuildIndex));
    }

    private IEnumerator LoadPlayerAtCheckpoint(int sceneBuildIndex)
    {
        //Fade in the loading screen + disable other screens
        RandomizeFunnyText();
        m_loadingCanva.SetActive(true);
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
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneBuildIndex);
        TryToGetVariables();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {

        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {

        }

        //Fade out the loading Screen
        m_loadingCanva.SetActive(false);
        m_loadingScreen.SetActive(false);
    }

    private void RandomizeFunnyText()
    {
        string text = m_funnyTextOptions[Random.Range(0, m_funnyTextOptions.Length - 1)];
        m_funnyText.text = text;
    }
}