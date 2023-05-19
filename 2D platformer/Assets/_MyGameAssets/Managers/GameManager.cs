using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Device;

//This class controls the entire game flow
public class GameManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static GameManager instance;
    [SerializeField] private GameObject m_player;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private ParticleSystem m_spawnPS;
    [Header("Audio: ")]
    [SerializeField] private AudioClip m_buttonSFX;
    [SerializeField] private AudioClip m_spawnSFX;
    [SerializeField] private AudioClip m_pauseSFX;
    [Range(0.0f, 1.0f)][SerializeField] private float m_buttonSoundVolume;
    [Header("UI Screens: ")]
    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_gameScreen;
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private GameObject m_victoryScreen;
    [SerializeField] private GameObject m_itemScreen;
    [SerializeField] private CanvasGroup m_startScreenCG;
    [SerializeField] private CanvasGroup m_gameScreenCG;
    [SerializeField] private CanvasGroup m_loseScreenCG;
    [SerializeField] private CanvasGroup m_victoryScreenCG;
    [SerializeField] private CanvasGroup m_pauseScreenCG;
    [SerializeField] private CanvasGroup m_itemScreenCG;
    [Header("Item Screen UI elements: ")]
    [SerializeField] private TextMeshProUGUI m_itemTopText;
    [SerializeField] private TextMeshProUGUI m_itemNameText;
    [SerializeField] private TextMeshProUGUI m_itemLoreText;
    [SerializeField] private TextMeshProUGUI m_itemDescriptionText;
    [SerializeField] private Button m_itemContinueButton;
    [SerializeField] private Button m_victoryContinueButton;
    [SerializeField] private CanvasGroup m_coinCG;
    [SerializeField] private CanvasGroup m_timeCG;
    [SerializeField] private TextMeshProUGUI m_coinCountText;
    [SerializeField] private TextMeshProUGUI m_totalTimeText;
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

    #region Unity Functions

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

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
        // Scene setup
        Time.timeScale = 1.0f;
        SetState(GameState.start);
        SceneLoader.instance.FadeOut();
        m_startScreen.SetActive(true);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);
        m_startScreenCG.alpha = 1.0f;
        MovePlayer();
        m_teleportingObjects = GameObject.FindObjectsOfType<TeleportObject>();

        // Starting animations + starts the next coroutine
        StartCoroutine(JustLoadedTransition());
    }

    #endregion

    #region Getter and Setter methods

    public void SetState(GameState newState)
    {
        currentGameState = newState;
        Debug.Log("The current game state is: " + newState);
    }

    public GameState GetState()
    {
        return currentGameState;
    }

    public bool IsPlaying()
    {
        return currentGameState == GameState.playing;
    }

    #endregion

    public void SwitchPause()
    {
        m_audioSource.PlayOneShot(m_pauseSFX, m_buttonSoundVolume);

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

    #region Public button functions

    public void ContinueFromItemScreen()
    {
        m_audioSource.PlayOneShot(m_buttonSFX, m_buttonSoundVolume);
        RelicManager.instance.ShowRelics(3.0f);
        StartCoroutine(ContinuePlayingTransition(m_itemContinueButton, m_itemScreen, m_itemScreenCG));
    }

    public void ContinueFromVictoryScreen()
    {
        m_audioSource.PlayOneShot(m_buttonSFX, m_buttonSoundVolume);
        StartCoroutine(ContinuePlayingTransition(m_victoryContinueButton, m_victoryScreen, m_victoryScreenCG));
    }

    public void LoadScene(int sceneBuildIndex)
    {
        SceneLoader.instance.LoadScene(sceneBuildIndex);
    }

    public void LeavingScene(float fadeTime)
    {
        StartCoroutine(LeavingSceneTransition(fadeTime));
    }

    public void EndGame(CauseOfDeath causeOfDeath)
    {
        StartCoroutine(EndGameTransition(causeOfDeath));
    }

    public void WinGame()
    {
        PlayerPrefs.SetInt("PlayerWon", 1);
        StartCoroutine(WinGameTransition());
    }

    public void CollectItem(CollectibleItem collectibleItem)
    {
        StartCoroutine(CollectItemTransition(collectibleItem));
    }

    #endregion

    #region Miscellaneous functions

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
            if (m_teleportingObjects[i] != null)
            {
                m_teleportingObjects[i].JumpTeleport();
            }
        }
    }
    
    public void TeleportDashPlatforms()
    {
        for (int i = 0; i < m_teleportingObjects.Length; i++)
        {
            if (m_teleportingObjects[i] != null)
            {
                m_teleportingObjects[i].DashTeleport();
            }
        }
    }

    #endregion

    #region UI transitions

    public IEnumerator CollectItemTransition(CollectibleItem collectibleItem)
    {
        m_rb.velocity = new Vector3(0, 0, 0);
        SetState(GameState.collectedItem);
        m_itemTopText.text = collectibleItem.topText;
        m_itemNameText.text = collectibleItem.itemName;
        m_itemLoreText.text = collectibleItem.loreText;
        m_itemDescriptionText.text = collectibleItem.descriptionText;
        m_itemContinueButton.interactable = false;

        // Wait before fading in the item description
        yield return new WaitForSeconds(0.5f);
        m_gameScreenCG.DOFade(0.0f, 1.0f);
        yield return new WaitForSeconds(0.8f);
        m_itemScreen.SetActive(true);
        m_itemScreenCG.alpha = 0.0f;
        m_itemScreenCG.DOFade(1.0f, 2.5f);
        yield return new WaitForSeconds(2.5f);
        m_itemContinueButton.interactable = true;
    }

    private IEnumerator JustLoadedTransition()
    {
        yield return new WaitForSeconds(0.8f);
        m_startScreenCG.DOFade(0.0f, 1.0f);
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(StartingGameTransition(2.0f));
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

        // Watch yourself explode with no distractions
        yield return new WaitForSeconds(0.5f);
        m_gameScreenCG.DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.4f);

        // Fade out game screen and fade in lose screen
        m_loseScreenCG.alpha = 0.0f;
        m_loseScreen.SetActive(true);
        m_loseScreenCG.DOFade(1f, 0.8f);

        // Wait for animations to end 
        yield return new WaitForSeconds(1.0f);
        MovePlayer();

        // Wait a little before fading back out
        yield return new WaitForSeconds(0.3f);
        m_loseScreenCG.DOFade(0.0f, 0.6f).OnComplete(() =>
        {
            m_loseScreen.SetActive(false);
        });

        StartCoroutine(StartingGameTransition(0.6f));
    }

    private IEnumerator WinGameTransition()
    {
        m_rb.velocity = new Vector3(0, 0, 0);
        m_victoryScreen.SetActive(true);
        m_victoryScreenCG.alpha = 0.0f;
        m_coinCG.alpha = 0.0f;
        m_timeCG.alpha = 0.0f;
        m_victoryContinueButton.enabled = false;
        m_coinCountText.text = CoinManager.instance.GetCoinCount().ToString() + " / 35";

        // Time elapsed:
        float totalSec = PlayerPrefs.GetFloat("FinishedTime");
        int hours = (int)(totalSec / 3600);
        int minutes = (int)((totalSec - (hours * 3600)) / 60);
        int sec = (int)(totalSec - (hours * 3600) - (minutes * 60));
        m_totalTimeText.text = hours + "h " + minutes + "min " + sec + "sec";

        SetState(GameState.win);
        m_gameScreenCG.DOFade(0.0f, 1.0f);
        yield return new WaitForSeconds(0.8f);
        m_victoryScreenCG.DOFade(1.0f, 2.5f);
        yield return new WaitForSeconds(3.0f);
        m_coinCG.DOFade(1.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        m_timeCG.DOFade(1.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        m_victoryContinueButton.enabled = true;
    }

    private IEnumerator StartingGameTransition(float seconds = 1.0f)
    {
        // Resets the player, waits for the time you inputed and then you can start playing
        PlayerManager.instance.StartGame();

        // Fade in game UI before playing:
        m_gameScreen.SetActive(true);
        m_gameScreenCG.alpha = 0.0f;
        m_gameScreenCG.DOFade(1.0f, seconds);
        yield return new WaitForSeconds(seconds);
        m_spawnPS.Play();
        m_audioSource.PlayOneShot(m_spawnSFX);
        SetState(GameState.playing);
    }

    private IEnumerator ContinuePlayingTransition(Button continueButton, GameObject screen, CanvasGroup screenCG)
    {
        continueButton.interactable = false;
        screenCG.DOFade(0.0f, 1.0f);
        yield return new WaitForSeconds(0.8f);
        screen.SetActive(true);
        screenCG.alpha = 0.0f;
        m_gameScreenCG.DOFade(1.0f, 0.8f);
        yield return new WaitForSeconds(0.4f);
        screen.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        SetState(GameState.playing);
    }

    private IEnumerator LeavingSceneTransition(float fadeTime)
    {
        m_pauseScreenCG.DOFade(0.0f, fadeTime).SetUpdate(true);
        yield return new WaitForSeconds(fadeTime);
        m_pauseScreenCG.DOKill();
        m_startScreen.SetActive(false);
        m_gameScreen.SetActive(false);
        m_pauseScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        m_victoryScreen.SetActive(false);
        m_itemScreen.SetActive(false);
    }

    #endregion

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