using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

//This class controls the entire game flow
public class GameManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static GameManager instance;
    [SerializeField] private GameObject m_player;
    [SerializeField] private Rigidbody2D m_rb;
    [Header("UI Screens: ")]
    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_gameScreen;
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private GameObject m_victoryScreen;
    [SerializeField] private GameObject m_itemScreen;
    [Header("Item Screen UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_itemTopText;
    [SerializeField] private TextMeshProUGUI m_itemNameText;
    [SerializeField] private TextMeshProUGUI m_itemLoreText;
    [SerializeField] private TextMeshProUGUI m_itemDescriptionText;
    [Header("End screen UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_causeOfDeathText;
    [SerializeField] private TextMeshProUGUI m_smallerText;
    [Header("Other: ")]
    [SerializeField] private GameState currentGameState;
    [Header("Testing? ")]
    [SerializeField] private bool m_isTesting;
    [Header("Last checkpoint: ")]
    [SerializeField] private GameObject m_lastCheckpoint;

    //This represents the possible game states
    public enum GameState
    {
        start,
        playing,
        collectedItem,
        paused,
        lose,
        win
    }

    public enum Item
    {
        dashPotion,
        airJumpPotion,
        wallJumpPotion,
        cloverRelic,
        scrollRelic,
        coinRelic,
        crystalRelic,
        bookRelic,
        crownRelic,
    }

    public enum CauseOfDeath
    {
        darkness,
        acid,
        enemy,
        insideObject
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Last_Checkpoint"))
        {
            m_lastCheckpoint = GameObject.Find(PlayerPrefs.GetString("Last_Checkpoint"));
        }
        else
        {
            m_lastCheckpoint = GameObject.Find("Starting_Checkpoint");
            PlayerPrefs.SetString("Last_Checkpoint", m_lastCheckpoint.name);
        }

        Time.timeScale = 1.0f;
        SetState(GameState.start);
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);

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
        MovePlayer();
        PlayerManager.instance.StartGame();
        StartPlaying();
    }

    public void StartPlaying()
    {
        SetState(GameState.playing);
        m_gameScreen.SetActive(true);
        m_startScreen.SetActive(false);
    }

    public void ContinuePlaying()
    {
        currentGameState = GameState.playing;
        m_itemScreen.SetActive(false);
        m_gameScreen.SetActive(true);
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
        m_itemScreen.SetActive(false);
    }

    public void EndGame(CauseOfDeath causeOfDeath)
    {
        StartCoroutine(EndGameTransition(causeOfDeath));
    }

    public void CollectItem(CollectibleItem collectibleItem)
    {
        m_rb.velocity = new Vector3(0, 0, 0);
        currentGameState = GameState.collectedItem;
        m_gameScreen.SetActive(false);
        m_itemScreen.SetActive(true);

        m_itemTopText.text = collectibleItem.topText;
        m_itemNameText.text = collectibleItem.itemName;
        m_itemLoreText.text = collectibleItem.loreText;
        m_itemDescriptionText.text = collectibleItem.descriptionText;
    }

    public void SetLastCheckpoint(GameObject newCheckpoint)
    {
        m_lastCheckpoint = newCheckpoint;
        PlayerPrefs.SetString("Last_Checkpoint", newCheckpoint.name);
    }

    private void MovePlayer()
    {
        if (m_isTesting)
        {
            m_player.transform.position = GameObject.Find("Testing_SpawnPosition").transform.position;
        }
        else
        {
            m_player.transform.position = m_lastCheckpoint.transform.position;
        }

        m_rb.velocity = new Vector3(0,0,0);
    }

    private IEnumerator EndGameTransition(CauseOfDeath causeOfDeath)
    {
        if (PlayerPrefs.HasKey("Death_Count"))
        {
            PlayerPrefs.SetInt("Death_Count", PlayerPrefs.GetInt("Death_Count") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Death_Count", 1);
        }

        if (causeOfDeath == CauseOfDeath.darkness)
        {
            m_causeOfDeathText.text = "Consumed by darkness.";
            m_smallerText.text = "Next time, don't get lost in the dark.";
        }
        else if (causeOfDeath == CauseOfDeath.acid)
        {
            m_causeOfDeathText.text = "Burned by corrosive acid.";
            m_smallerText.text = "Who knew corrosive acid could be dangerous?";
        }
        else if (causeOfDeath == CauseOfDeath.enemy)
        {
            m_causeOfDeathText.text = "Touched something pointy";
            m_smallerText.text = "Stop doing that!";
        }
        else if (causeOfDeath == CauseOfDeath.insideObject)
        {
            m_causeOfDeathText.text = "A platform spawned on you";
            m_smallerText.text = "You're cut in half now.";
        }

        PlayerPrefs.Save();
        SetState(GameState.lose);
        m_gameScreen.SetActive(false);
        m_loseScreen.SetActive(true);
        RelicManager.s_showRelics = true;

        yield return new WaitForSeconds(1.5f);

        MovePlayer();

        yield return new WaitForSeconds(0.2f);
        
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);

        PlayerManager.instance.StartGame();
    }
}