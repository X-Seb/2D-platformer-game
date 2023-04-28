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
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                m_mainMenuManager = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();
                break;
            case 1:
                m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                break;
            default:
                break;
        }
    }

    public void LoadScene(int sceneBuildIndex)
    {
        TryToGetVariables();

        if (m_loadAsync)
        {
            StartCoroutine(LoadSceneAsync(sceneBuildIndex, 2.0f));
        }
        else
        {
            StartCoroutine(LoadSceneDefault(sceneBuildIndex, 2.0f));
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutTransition());
    }

    private IEnumerator LoadSceneDefault(int targetSceneIndex, float fadeTime)
    {
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

        // Wait before fading-in the loading screen
        yield return new WaitForSecondsRealtime(0.8f);
        RandomizeFunnyText();
        m_loadingCanva.SetActive(true);
        m_loadingScreen.SetActive(true);
        m_loadingScreenCG.alpha = 0.0f;
        m_loadingScreenCG.DOFade(1.0f, fadeTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(fadeTime);
        SceneManager.LoadScene(targetSceneIndex);
    }

    private IEnumerator LoadSceneAsync(int targetSceneIndex, float fadeTime)
    {
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

        // Start loading the scene asynchronously
        AsyncOperation scene = SceneManager.LoadSceneAsync(targetSceneIndex);
        scene.allowSceneActivation = false;

        // Wait before fading-in the loading screen
        yield return new WaitForSecondsRealtime(0.8f);
        RandomizeFunnyText();
        m_loadingCanva.SetActive(true);
        m_loadingScreen.SetActive(true);
        m_loadingScreenCG.alpha = 0.0f;
        m_loadingScreenCG.DOFade(1.0f, fadeTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.2f);

        // Wait until the scene is loaded
        while (!scene.isDone)
        {
            if (scene.progress >= 0.9f)
            {
                scene.allowSceneActivation = true;
            }
            yield return null;
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