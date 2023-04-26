using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

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
    [Header("For reference only: ")]
    [SerializeField] private GameObject m_lastCheckpoint;
    [SerializeField] private TeleportObject[] m_teleportingObjects;

    #region enums

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
        insideObject,
        fire
    }
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (PlayerPrefs.HasKey("Last_Checkpoint"))
        {
            m_lastCheckpoint = GameObject.Find(PlayerPrefs.GetString("Last_Checkpoint"));
        }
        else
        {
            m_lastCheckpoint = GameObject.Find("1_StartingCheckpoint");
            PlayerPrefs.SetString("Last_Checkpoint", m_lastCheckpoint.name);
        }
    }

    private void Start()
    {

        Time.timeScale = 1.0f;
        SetState(GameState.start);
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);

        MovePlayer();

        // Get all the teleporting objects
        m_teleportingObjects = GameObject.FindObjectsOfType<TeleportObject>();

        StartCoroutine(StartingGameTransition(3.0f));
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

        //TODO fade out game UI and fade in item screen

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

    public void TeleportJumpPlatforms()
    {
        for (int i = 0; i < m_teleportingObjects.Length; i++)
        {
            m_teleportingObjects[i].JumpTeleport();
        }
    }
    
    public void TeleportDashPlatforms()
    {
        for (int i = 0; i < m_teleportingObjects.Length; i++)
        {
            m_teleportingObjects[i].DashTeleport();
        }
    }

    private IEnumerator EndGameTransition(CauseOfDeath causeOfDeath)
    {
        // Adjust the death count
        if (PlayerPrefs.HasKey("Death_Count"))
        {
            PlayerPrefs.SetInt("Death_Count", PlayerPrefs.GetInt("Death_Count") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Death_Count", 1);
        }

        // Display the right text on the death screen
        if (causeOfDeath == CauseOfDeath.darkness)
        {
            m_causeOfDeathText.text = "Consumed by darkness.";
            m_smallerText.text = "Next time, don't get lost in the dark.";
        }
        else if (causeOfDeath == CauseOfDeath.acid)
        {
            m_causeOfDeathText.text = "Melted by corrosive acid.";
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
        else if (causeOfDeath == CauseOfDeath.fire)
        {
            m_causeOfDeathText.text = "You got burned!";
            m_smallerText.text = "That's hot!";
        }

        PlayerPrefs.Save();
        SetState(GameState.lose);

        // TODO fade out game screen and fade in lose screen

        m_loseScreen.SetActive(true);
        CanvasGroup loseScreen = m_loseScreen.GetComponent<CanvasGroup>();
        loseScreen.alpha = 0.0f;
        yield return loseScreen.DOFade(1f, 0.5f).WaitForCompletion();

        m_gameScreen.SetActive(false);

        // Watch yourself explode before going back to the last checkpoint
        yield return new WaitForSeconds(1.5f);
        MovePlayer();

        // Activate the starting UI and the start playing in 1 second
        //yield return new WaitForSeconds(0.2f);
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);
        StartCoroutine(StartingGameTransition(1.0f));
    }

    private IEnumerator StartingGameTransition(float seconds = 1.0f)
    {
        // Resets the player, waits for the time you inputed and then you can start playing
        PlayerManager.instance.StartGame();

        //TODO fade in game UI

        yield return new WaitForSeconds(seconds);
        StartPlaying();
    }

    public IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    #region Editor stuff

    public void SwitchTesting()
    {
        if (m_isTesting)
        {
            m_isTesting = false;
        }
        else
        {
            m_isTesting = true;
        }
    }

    #endregion
}