using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This class controls the entire game flow
public class GameManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static GameManager instance;
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
        // Restart the game at the exact same checkpoint
    }

    public void LoadGameAtCheckpoint(int checkpointID)
    {
        //Load the game scene and place the player at the location of the last checkpoint
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Go back to Main Menu");
    }

    public void LoadRewardScene()
    {
        //SceneManager.LoadScene(2);
        Debug.Log("Go to the final scene for a reward!");
    }

    public void StartPlaying()
    {
        SetState(GameState.playing);
        m_gameScreen.SetActive(true);
        m_startScreen.SetActive(false);
    }

    public void EndGame()
    {
        SetState(GameState.lose);
        m_gameScreen.SetActive(false);
        m_loseScreen.SetActive(true);

        //StartCoroutine(EndGameTransition());
    }

    private IEnumerator EndGameTransition()
    {
        yield return null;
    }
}