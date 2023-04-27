using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [Header("Loading method:")]
    [SerializeField] private bool m_loadAsync;
    [Header("UI stuff: ")]
    [SerializeField] private GameObject m_loadingCanva;
    [SerializeField] private GameObject m_loadingScreen;
    [SerializeField] private CanvasGroup m_loadingScreenCG;
    [SerializeField] private TextMeshProUGUI m_funnyText;
    [Header("Checkpoint stuff: ")]
    [SerializeField] private int m_lastCheckpoint;
    [Header("Main menu: ")]
    [SerializeField] private MainMenuManager m_mainMenuManager;
    [Header("Other: ")]
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
    }

    public void LoadScene(int sceneBuildIndex)
    {
        TryToGetVariables();
        StartCoroutine(LoadPlayerAtCheckpoint(sceneBuildIndex, 2.0f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutTransition());
    }

    private IEnumerator LoadPlayerAtCheckpoint(int targetSceneIndex, float fadeTime)
    {
        AsyncOperation scene;

        // Start fading out the current scene UI
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                m_mainMenuManager.LeavingScene(1.0f);
                break;
            case 1:
                m_gameManager.LeavingScene(1.0f);
                break;
            default:
                break;
        }

        if (m_loadAsync)
        {
            scene = SceneManager.LoadSceneAsync(targetSceneIndex);
        }

        // Wait before fading-in the loading screen
        yield return new WaitForSecondsRealtime(0.8f);
        RandomizeFunnyText();
        m_loadingCanva.SetActive(true);
        m_loadingScreen.SetActive(true);
        m_loadingScreenCG.alpha = 0.0f;
        m_loadingScreenCG.DOFade(1.0f, fadeTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(fadeTime);

        if (m_loadAsync)
        {
            //scene.allowSceneActivation = true;
        }
        else
        {
            SceneManager.LoadScene(targetSceneIndex);
            TryToGetVariables();
        }
    }

    private IEnumerator FadeOutTransition()
    {
        TryToGetVariables();
        m_loadingScreenCG.DOFade(0.0f, 1.0f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.0f);
        m_loadingScreen.SetActive(false);
    }

    private void RandomizeFunnyText()
    {
        string text = m_funnyTextOptions[Random.Range(0, m_funnyTextOptions.Length - 1)];
        m_funnyText.text = text;
    }
}