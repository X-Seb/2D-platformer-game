using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This class controls the entire game flow
public class GameManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static GameManager instance;
    [SerializeField] private GameObject m_player;
    [Header("UI: ")]
    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_gameScreen;
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private GameObject m_victoryScreen;
    [Header("Other: ")]
    [SerializeField] private GameState currentGameState;

    //This represents the possible game states
    public enum GameState
    {
        start,
        playing,
        paused,
        lose,
        win
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetState(GameState.start);
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);

        MovePlayer();
    }

    public void SetState(GameState newState)
    {
        currentGameState = newState;
        Debug.Log("The current game state is: " + newState);
    }

    public GameState GetState()
    {
        return currentGameState;
    }

    public void SwitchPause()
    {
        if (currentGameState == GameState.paused)
        {
            ResumeGame();
        }
        else if(currentGameState == GameState.playing)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        SetState(GameState.paused);
        Time.timeScale = 0.0f;
        m_pauseScreen.SetActive(true);
        m_gameScreen.SetActive(false);
    }

    private void ResumeGame()
    {
        SetState(GameState.playing);
        Time.timeScale = 1.0f;
        m_pauseScreen.SetActive(false);
        m_gameScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneLoader.instance.LoadScene(1);
    }

    public void StartPlaying()
    {
        SetState(GameState.playing);
        m_gameScreen.SetActive(true);
        m_startScreen.SetActive(false);
    }

    public void LoadScene(int sceneBuildIndex)
    {
        SceneLoader.instance.LoadScene(sceneBuildIndex);
    }

    public void LeavingScene()
    {
        m_startScreen.SetActive(false);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
    }

    public void EndGame()
    {
        if (PlayerPrefs.HasKey("Death_Count"))
        {
            PlayerPrefs.SetInt("Death_Count", PlayerPrefs.GetInt("Death_Count") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Death_Count", 1);
        }

        PlayerPrefs.Save();

        SetState(GameState.lose);
        m_gameScreen.SetActive(false);
        m_loseScreen.SetActive(true);
        RelicManager.s_showRelics = true;

        StartCoroutine(EndGameTransition());
    }

    private void MovePlayer()
    {
        if (!PlayerPrefs.HasKey("Last_Checkpoint"))
        {
            PlayerPrefs.SetInt("Last_Checkpoint", 1);
        }
        int checkpoint = PlayerPrefs.GetInt("Last_Checkpoint");
        m_player.transform.position = GameObject.Find("Checkpoint_" + checkpoint).transform.position;
    }

    private IEnumerator EndGameTransition()
    {
        yield return new WaitForSeconds(1.5f);

        SceneLoader.instance.LoadScene(1);
    }
}