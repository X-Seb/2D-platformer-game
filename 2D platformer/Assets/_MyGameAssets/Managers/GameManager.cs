using UnityEngine;

//This class controls the entire game flow
public class GameManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static GameManager instance;
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

    public void TryToPause()
    {
        if (currentGameState == GameState.playing)
        {
            PauseGame();
        }
    }

    public void TryToResume()
    {
        if (currentGameState == GameState.paused)
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        SetState(GameState.paused);
    }

    private void ResumeGame()
    {
        SetState(GameState.playing);
    }
}