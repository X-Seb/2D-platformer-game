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
    [Header("item Screen UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_itemNameText;
    [SerializeField] private TextMeshProUGUI m_itemLoreText;
    [SerializeField] private TextMeshProUGUI m_itemDescriptionText;
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
        unlockingAbility,
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

    public void EndGame()
    {
        StartCoroutine(EndGameTransition());
    }

    public void CollectItem(string itemName)
    {
        currentGameState = GameState.unlockingAbility;
        m_gameScreen.SetActive(false);
        m_itemScreen.SetActive(true);

        if (itemName == "Dash")
        {
            m_itemNameText.text = "Burst Potion";
            m_itemLoreText.text = "A mysterious potion that grants its user the ability to burst forward.";
            m_itemDescriptionText.text = "Press S, G, down arrow, or SHIFT to dash in the direction you're facing.";
        }
        else if (itemName == "AirJump")
        {
            m_itemNameText.text = "Levitation Potion";
            m_itemLoreText.text = "A strange potion that grants its user the ability to jump a second time while in the air.";
            m_itemDescriptionText.text = "Press W, SPACE, or the up arrow to jump in midair.";
        }
        else if (itemName == "WallJump")
        {
            m_itemNameText.text = "Sticky Potion";
            m_itemLoreText.text = "A smelly potion that grants its user the ability to jump off of walls they're holding on to.";
            m_itemDescriptionText.text = "Pressing W, SPACE, or the up arrow while holding onto a wall will propell you off in the opposite direction.";
        }
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

    private IEnumerator EndGameTransition()
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