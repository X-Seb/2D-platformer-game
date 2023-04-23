using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Setup: ")]
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private TrailRenderer m_trailRenderer;
    [SerializeField] private TrailRenderer m_mainTrailRenderer;
    [SerializeField] private Animator m_animator;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_movingPlatformLayer;
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private Camera m_cam;
    [Header("Upgrading the player: ")]
    [SerializeField] private bool m_isDashUnlocked;
    [SerializeField] private bool m_isAirJumpUnlocked;
    [SerializeField] private bool m_isWallSlideUnlocked;
    [SerializeField] private bool m_isWallJumpUnlocked;
    [SerializeField] private bool m_isAcidImmunityUnlocked;
    [Header("Movement: ")]
    [Range(0, 200)][SerializeField] private float m_playerMoveSpeed = 80f; // How fast the player can move.
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool m_isAirControlAllowed = false; // If the player can move horizontally while in the air.
    [SerializeField] private float m_maxFallVelocity;
    [SerializeField] private Vector3 m_targetMoveVelocity = Vector3.zero;
    [Header("Jumping: ")]
    [Range(0, 50)][SerializeField] private float m_jumpForce = 25f; // Amount of force added when the player jumps.
    [Range(0, 50)][SerializeField] private float m_airJumpForce = 15f; // Amount of force added when the player jumps in the air.
    [SerializeField] private bool m_isInfiniteAirJumpsAllowed = false;
    [Range(0, 10)][SerializeField] private int m_maxNumberOfAirJumps = 1;
    [Header("Dashing: ")]
    [SerializeField] private bool m_isInfiniteDashAllowed = false;
    [Range(0, 10)][SerializeField] private int m_maxNumberOfDash = 1;
    [Range(0, 50)][SerializeField] private float m_dashForce = 25f;
    [Range(0, 2)][SerializeField] private float m_dashDuration = 0.2f;
    [Range(0, 10)][SerializeField] private float m_dashingCooldownTime = 1.5f;
    [SerializeField] private bool m_canDash = true;
    [SerializeField] private bool m_isDashing = false;
    [Header("Wall sliding: ")]
    [SerializeField] private bool m_isWallSliding;
    [SerializeField] private float m_wallSlidingSpeed;
    [SerializeField] private Transform m_wallCheck1;
    [SerializeField] private Transform m_wallCheck2;
    [SerializeField] private LayerMask m_wallLayer;
    [Header("Wall jumping: ")]
    [SerializeField] private bool m_isWallJumping;
    [SerializeField] private bool m_canWallJump;
    [SerializeField] private float m_wallJumpingDirection;
    [SerializeField] private float m_wallJumpingDuration = 0.3f;
    [SerializeField] private Vector2 m_wallJumpingPower = new Vector2(8.0f, 16.0f);
    [Header("Reference information: ")]
    [SerializeField] private bool m_isGrounded;  // Whether or not the player is grounded.
    [SerializeField] private const float k_GroundedRadius = 0.4f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private bool m_isOnPlatform;
    [SerializeField] private bool m_isOnWall;
    [SerializeField] private bool m_isFacingRight = true;
    [SerializeField] private bool m_isJumping;
    [SerializeField] private int m_numberOfAirJumps = 0;
    [SerializeField] private int m_numberOfDash = 0;
    [SerializeField] private float m_axisX = 0f; // Either -1, 0, or 1
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_playerAudioSource;
    [SerializeField] private AudioClip m_deathAudioClip;
    [SerializeField] private AudioClip m_jumpAudioClip;
    [SerializeField] private AudioClip m_landAudioClip;
    [SerializeField] private AudioClip m_airJumpAudioClip;
    [SerializeField] private AudioClip m_dashAudioClip;
    [SerializeField] private AudioClip m_bouncyAudioClip;
    [Header("Player light: ")]
    [SerializeField] private Slider m_lightSlider;
    [SerializeField] private Light2D m_playerLight;
    [SerializeField] private bool m_isLightIncreasing;
    [SerializeField] private float m_lightPercentage;
    [SerializeField] private float m_maxLightIntensity;
    [SerializeField] private float m_minLightIntensity;
    [SerializeField] private float m_minLightOuterRadius;
    [SerializeField] private float m_maxLightOuterRadius;
    [SerializeField] private float m_increasingLightSpeed;
    [Range(0.001f, 0.1f)][SerializeField] private float m_decreasingLightSpeedEasy;
    [Range(0.001f, 0.1f)][SerializeField] private float m_decreasingLightSpeedMedium;
    [Range(0.001f, 0.1f)][SerializeField] private float m_decreasingLightSpeedHard;
    [SerializeField] private float m_decreasingLightSpeedDead;
    [SerializeField] private float m_currentLightIntensity;
    [SerializeField] private float m_currentLightOuterRadius;
    [Header("Effects: ")]
    [SerializeField] private bool m_playDeathCamFX;
    [SerializeField] private bool m_playDashCamFX;
    [SerializeField] private bool m_isMainTrailEmmiting = false;
    [SerializeField] private Color m_pinkTrailColor;
    [SerializeField] private Color m_playerAirJumpColor;
    [SerializeField] private float m_dashZoomAmount;
    [SerializeField] private float m_zoomDuration;
    [SerializeField] private float m_collisionDuration;
    [SerializeField] private Vector3 m_collisionStrength;
    [SerializeField] private int m_collisionVibrato;
    [SerializeField] private float m_collisionRandomness;
    [Header("Events: ")]
    [SerializeField] private UnityEvent m_dashRegainedEvent;
    [SerializeField] private UnityEvent m_dashEvent;
    [SerializeField] private UnityEvent m_jumpEvent;
    [SerializeField] private UnityEvent m_airJumpEvent;
    [SerializeField] private UnityEvent m_wallJumpEvent;
    [SerializeField] private UnityEvent m_airJumpRegainedEvent;
    [SerializeField] private UnityEvent m_groundedRegainedEvent;
    [SerializeField] private UnityEvent m_diedEvent;

    public enum SoundType
    {
        bouncy
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        m_isOnWall = IsTouchingWall();
        m_isOnPlatform = IsOnPlatform();
        m_axisX = InputManager.instance.ReturnAxisX();
        AnimatePlayer();
        AjustPlayerLight();
        AdjustTrailColor(false);
        AdjustPlayerColor();

        if (!m_isMainTrailEmmiting && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_mainTrailRenderer.emitting = true;
            m_isMainTrailEmmiting = true;
        }
        else if (m_isMainTrailEmmiting && (GameManager.instance.GetState() != GameManager.GameState.playing || m_isDashing))
        {
            m_mainTrailRenderer.emitting = false;
            m_isMainTrailEmmiting = false;
        }

        if (m_Rigidbody2D.velocity.y < m_maxFallVelocity)
        {
            m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, m_maxFallVelocity, 0);
        }
    }

    private void FixedUpdate()
    {
        IsOnGround();
        TryToWallSlide();
        Move(m_axisX * Time.fixedDeltaTime * m_playerMoveSpeed);

        if (!m_isWallJumping && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            TryToFlip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            PlayerDied(GameManager.CauseOfDeath.enemy);
        }
        else if (collision.gameObject.CompareTag("Platform") && m_isOnPlatform)
        {
            gameObject.transform.parent = collision.transform;
            m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 0, 0);
            m_isOnPlatform = true;
            //m_isJumping = false;
        }
        else if (collision.gameObject.CompareTag("Inside Of Object") && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            PlayerDied(GameManager.CauseOfDeath.insideObject);
        }
        else if ((collision.gameObject.CompareTag("Fire") || collision.gameObject.CompareTag("FireOnCandle"))
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            PlayerDied(GameManager.CauseOfDeath.fire);
        }
        else if (collision.gameObject.CompareTag("Ground") && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_isJumping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && m_isOnPlatform)
        {
            gameObject.transform.parent = collision.transform;
            m_isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            m_isOnPlatform = false;
            gameObject.transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            m_isLightIncreasing = true;
        }
        else if (collision.CompareTag("Acid") && !m_isAcidImmunityUnlocked && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            PlayerDied(GameManager.CauseOfDeath.acid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            m_isLightIncreasing = false;
        }
    }

    public void StartGame()
    {
        m_Rigidbody2D.velocity = new Vector2(0, 0);
        GameManager.instance.SetState(GameManager.GameState.start);
        m_animator.SetBool("isDead", false);
        m_canWallJump = true;
        m_isJumping = false;
        m_lightPercentage = 1;
        UpdatePlayerPowers();
        AdjustTrailColor(false);
        AdjustPlayerColor();

        // Adjust player prefs if you've never played before
        if (!PlayerPrefs.HasKey("Jumps_Count"))
        {
            PlayerPrefs.SetInt("Jumps_Count", 0);
        }

        if (!PlayerPrefs.HasKey("Dashes_Count"))
        {
            PlayerPrefs.SetInt("Dashes_Count", 0);
        }

        if (!PlayerPrefs.HasKey("AirJumps_Count"))
        {
            PlayerPrefs.SetInt("AirJumps_Count", 0);
        }
    }

    private void PlayerDied(GameManager.CauseOfDeath causeOfDeath)
    {
        GameManager.instance.SetState(GameManager.GameState.lose);
        m_animator.SetBool("isDead", true);
        m_diedEvent.Invoke();

        if (m_playDeathCamFX)
        {
            m_cam.DOKill();
            m_cam.DOOrthoSize(7, m_zoomDuration).SetEase(Ease.InCubic);
            m_cam.DOShakeRotation(m_collisionDuration, m_collisionStrength, m_collisionVibrato, m_collisionRandomness, true, ShakeRandomnessMode.Harmonic).OnComplete(() =>
            {
                m_cam.DOOrthoSize(9, 0.7f).SetEase(Ease.InOutSine);
            });
        }

        GameManager.instance.EndGame(causeOfDeath);
    }

    private void AnimatePlayer()
    {
        m_animator.SetFloat("x-speed", Mathf.Abs(m_Rigidbody2D.velocity.x));
        m_animator.SetFloat("y-velocity", m_Rigidbody2D.velocity.y);
        m_animator.SetBool("isGrounded", m_isGrounded);
        m_animator.SetFloat("x-input", m_axisX);
        m_animator.SetBool("isOnWall", m_isWallSliding);
    }

    private void AjustPlayerLight()
    {
        // Increase the light percentage if your light should be increasing
        if (m_isLightIncreasing && m_lightPercentage < 1 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_lightPercentage += Time.deltaTime * m_increasingLightSpeed;
            if (m_lightPercentage >= 1)
            {
                m_lightPercentage = 1;
            }
        }

        // Decrease light % if you're not gaining any light + you're on easy mode
        else if (!m_isLightIncreasing && PlayerPrefs.GetInt("Difficulty") == 0 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_lightPercentage -= Time.deltaTime * m_decreasingLightSpeedEasy;
            if (m_lightPercentage <= 0)
            {
                m_lightPercentage = 0;
            }
        }

        // Decrease the light percentage if you're not gaining any light (reminder: 2 is easy, 1 is medium, 0 is hard)
        else if (!m_isLightIncreasing && PlayerPrefs.GetInt("Difficulty") == 1 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_lightPercentage -= Time.deltaTime * m_decreasingLightSpeedMedium;
            if (m_lightPercentage <= 0)
            {
                m_lightPercentage = 0;
            }
        }

        // Decrease the light percentage if you're not gaining any light + difficulty is set to hard
        else if (!m_isLightIncreasing && PlayerPrefs.GetInt("Difficulty") == 0 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_lightPercentage -= Time.deltaTime * m_decreasingLightSpeedHard;
            if (m_lightPercentage <= 0)
            {
                m_lightPercentage = 0;
            }
        }

        else if (m_lightPercentage >= 0 && GameManager.instance.GetState() == GameManager.GameState.lose)
        {
            m_lightPercentage -= Time.deltaTime * m_decreasingLightSpeedDead;
            if (m_lightPercentage <= 0)
            {
                m_lightPercentage = 0;
            }
        }

        // If you're at 0% of light, you die
        if (m_lightPercentage <= 0 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            PlayerDied(GameManager.CauseOfDeath.darkness);
        }
        else
        {
            // If you're not at 0%, ajust the player's light
            m_currentLightIntensity = Mathf.Lerp(m_minLightIntensity, m_maxLightIntensity, m_lightPercentage);
            m_playerLight.intensity = m_currentLightIntensity;
            m_currentLightOuterRadius = Mathf.Lerp(m_minLightOuterRadius, m_maxLightOuterRadius, m_lightPercentage);
            m_playerLight.pointLightOuterRadius = m_currentLightOuterRadius;
            m_lightSlider.value = m_lightPercentage;
        }
    }

    private void AdjustTrailColor(bool playEffects = true)
    {
        if (m_canDash && m_isDashUnlocked && (m_numberOfDash >= 1 || m_isInfiniteDashAllowed) && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_mainTrailRenderer.startColor = m_pinkTrailColor;
            m_mainTrailRenderer.endColor = m_pinkTrailColor;
            if (playEffects)
            {
                m_dashRegainedEvent.Invoke();
            }
        }
    }

    private void AdjustPlayerColor()
    {
        if (m_isAirJumpUnlocked && (m_isInfiniteAirJumpsAllowed || m_numberOfAirJumps >= 1))
        {
            m_spriteRenderer.material.color = m_playerAirJumpColor;
        }
        else
        {
            m_spriteRenderer.material.color = new Color(1, 1, 1);
        }
    }

    public void PlaySound(SoundType soundType, float volume = 1.0f)
    {
        switch (soundType)
        {
            case SoundType.bouncy:
                m_playerAudioSource.PlayOneShot(m_bouncyAudioClip, volume);
                break;
            default:
                break;
        }
    }

    private void Move(float move)
    {
        //only control the player if grounded or airControl is turned on, you're not dashing, and you're not in the process of wall jumping
        if ((m_isGrounded || m_isAirControlAllowed) && !m_isWallSliding && !m_isWallJumping &&!m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_targetMoveVelocity, m_MovementSmoothing);
        }
    }

    public void TryToJump(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>();
        
        // Jump from the ground
        if (ctx.performed && m_isGrounded && !m_isDashing && !m_isWallSliding && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_isGrounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_jumpForce);
            PlayerPrefs.SetInt("Jumps_Count", PlayerPrefs.GetInt("Jumps_Count") + 1);
            GameManager.instance.TeleportJumpPlatforms();
            m_jumpEvent.Invoke();
            m_isJumping = true;
        }

        // Jump in mid-air
        else if (ctx.performed && m_isAirJumpUnlocked && (m_isInfiniteAirJumpsAllowed || m_numberOfAirJumps > 0) && !m_isGrounded && !m_isDashing && !m_isWallJumping && !m_isWallSliding
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_airJumpForce);
            m_numberOfAirJumps--;
            PlayerPrefs.SetInt("AirJumps_Count", PlayerPrefs.GetInt("AirJumps_Count") + 1);
            GameManager.instance.TeleportJumpPlatforms();
            m_airJumpEvent.Invoke();
            m_isJumping = true;
        }

        // Jump while sliding down a wall
        else if (ctx.performed && m_isWallJumpUnlocked && m_canWallJump && m_isWallSliding && !m_isGrounded && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            StartCoroutine(WallJump());
        }

        // Jump from a wall, but use a air-jump (before you unlock wall-jump)
        else if (ctx.performed && m_canWallJump && m_isWallSliding &&
            m_isAirJumpUnlocked && (m_isInfiniteAirJumpsAllowed || m_numberOfAirJumps > 0) &&
            !m_isGrounded && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            StartCoroutine(WallJump());
        }

        else if (ctx.canceled && m_isJumping && m_Rigidbody2D.velocity.y > 0)
        {
            // If player is currently jumping => slow down the player 
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.4f);
        }
    }

    private void TryToWallSlide()
    {
        if (m_isWallSlideUnlocked && m_isOnWall && !m_isGrounded && !m_isDashing && m_axisX != 0)
        {
            m_isWallSliding = true;
            //TODO: Fix wall sliding!!!!
            m_Rigidbody2D.velocity = new Vector2(0, Mathf.Clamp(m_Rigidbody2D.velocity.y, -m_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            m_isWallSliding = false;
        }
    }

    public void TryToDash()
    {
        if (m_isDashUnlocked && (m_isInfiniteDashAllowed || m_numberOfDash > 0) && m_canDash
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            StartCoroutine(Dash());
            m_numberOfDash--;
        }
    }

    private void TryToFlip()
    {
        // Only flip if you're facing the opposite direction that you're tring to move in.
        if ((m_isFacingRight && m_axisX < 0f) || (!m_isFacingRight && m_axisX > 0f))
        {
            // Flip the player
            m_isFacingRight = !m_isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private IEnumerator Dash()
    {
        PlayerPrefs.SetInt("Dashes_Count", PlayerPrefs.GetInt("Dashes_Count") + 1);

        m_mainTrailRenderer.startColor = new Color(1, 1, 1);
        m_mainTrailRenderer.endColor = new Color(1, 1, 1);

        m_canDash = false;
        m_isDashing = true;
        m_animator.SetBool("isDashing", true);

        // Teleport the dash platforms:
        GameManager.instance.TeleportDashPlatforms();

        //Remove gravity for now
        float originalGravity = m_Rigidbody2D.gravityScale;
        m_Rigidbody2D.gravityScale = 0f;

        // Start dashing and make the trail appear behind you
        m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_dashForce, 0f);
        m_trailRenderer.emitting = true;
        m_playerAudioSource.PlayOneShot(m_dashAudioClip);
        m_dashEvent.Invoke();

        // Will make you zoom in while you dash and zoom out soon after
        if (m_playDashCamFX)
        {
            m_cam.DOKill();
            m_cam.DOOrthoSize(m_dashZoomAmount, m_dashDuration * 0.9f).OnComplete(() => {
                m_cam.DOOrthoSize(9, 0.3f);
            });
        }

        yield return new WaitForSeconds(m_dashDuration);

        m_trailRenderer.emitting = false;
        m_Rigidbody2D.gravityScale = originalGravity;
        m_isDashing = false;
        m_animator.SetBool("isDashing", false);

        // Wait for the cooldown to finish before allowing you to dash again
        yield return new WaitForSeconds(m_dashingCooldownTime);
        m_canDash = true;
        AdjustTrailColor();
    }

    private IEnumerator WallJump()
    {
        m_canWallJump = false;
        m_isWallSliding = false;
        m_isWallJumping = true;

        if (m_isFacingRight)
        {
            m_wallJumpingDirection = -1;
        }
        else
        {
            m_wallJumpingDirection = 1;
        }

        m_Rigidbody2D.velocity = new Vector2(m_wallJumpingDirection * m_wallJumpingPower.x, m_wallJumpingPower.y);
        m_wallJumpEvent.Invoke();
        m_isJumping = true;

        if (transform.localScale.x != m_wallJumpingDirection)
        {
            m_isFacingRight = !m_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        if (!m_isWallJumpUnlocked)
        {
            m_numberOfAirJumps--;
        }

        yield return new WaitForSeconds(m_wallJumpingDuration);

        m_canWallJump = true;
        m_isWallJumping = false;
    }

    private void IsOnGround()
    {
        bool wasGrounded = m_isGrounded;
        m_isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, k_GroundedRadius, m_groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_isGrounded = true;
                m_numberOfAirJumps = m_maxNumberOfAirJumps;
                m_numberOfDash = m_maxNumberOfDash;

                if (!wasGrounded && m_isGrounded && m_Rigidbody2D.velocity.y <= 0 &&
                    colliders[i].gameObject.GetComponent<BouncyObject>() == null && GameManager.instance.GetState() == GameManager.GameState.playing)
                {
                    m_isJumping = false;
                    m_groundedRegainedEvent.Invoke();
                    m_airJumpRegainedEvent.Invoke();
                }
            }
        }
    }

    private bool IsOnPlatform()
    {
        return Physics2D.OverlapCircle(m_groundCheck.position, k_GroundedRadius, m_movingPlatformLayer);
    }

    private bool IsTouchingWall()
    {
        
        bool isTouchingWall1 = Physics2D.OverlapCircle(m_wallCheck1.position, 0.2f, m_wallLayer);
        bool isTouchingWall2 = Physics2D.OverlapCircle(m_wallCheck2.position, 0.2f, m_wallLayer);
        if (isTouchingWall1 || isTouchingWall2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdatePlayerPowers()
    {
        if (PlayerPrefs.HasKey("AirJump_Unlocked"))
        {
            m_isAirJumpUnlocked = true;
        }

        if (PlayerPrefs.HasKey("Dash_Unlocked"))
        {
            m_isDashUnlocked = true;
        }

        if (PlayerPrefs.HasKey("WallJump_Unlocked"))
        {
            m_isWallJumpUnlocked = true;
        }
    }
}