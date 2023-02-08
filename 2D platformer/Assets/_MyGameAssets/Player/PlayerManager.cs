using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private TrailRenderer m_trailRenderer;
    [SerializeField] private Animator m_animator;
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;  // A position marking where to check if the player is grounded.
    [Header("Upgrading the player: ")]
    [SerializeField] private bool m_isDashUnlocked;
    [SerializeField] private bool m_isAirJumpUnlocked;
    [SerializeField] private bool m_isWallJumpUnlocked;
    [Header("Input: ")]
    [SerializeField] private float m_axisX = 0f; // Either -1, 0, or 1
    [Header("Movement: ")]
    [Range(0, 200)][SerializeField] private float m_playerMoveSpeed = 80f; // How fast the player can move.
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool m_isAirControlAllowed = false; // If the player can move horizontally while in the air.
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
    [Header("Wall sliding: ")]
    [SerializeField] private bool m_isWallSliding;
    [SerializeField] private float m_wallSlidingSpeed;
    [SerializeField] private Transform m_wallCheck;
    [SerializeField] private LayerMask m_wallLayer;
    [Header("Wall jumping: ")]
    [SerializeField] private bool m_isWallJumping;
    [SerializeField] private bool m_canWallJump;
    [SerializeField] private float m_wallJumpingDirection;
    [SerializeField] private float m_wallJumpingTime = 0.2f;
    [SerializeField] private float m_wallJumpingDuration = 0.3f;
    [SerializeField] private float m_wallJumpingCounter;
    [SerializeField] private Vector2 m_wallJumpingPower = new Vector2(8.0f, 16.0f);
    [Header("Reference information: ")]
    [SerializeField] private bool m_isGrounded;  // Whether or not the player is grounded.
    [SerializeField] private const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private bool m_isFacingRight = true; // For determining which way the player is currently facing.
    [SerializeField] private int m_numberOfAirJumps = 0;
    [SerializeField] private bool m_canDash = true;
    [SerializeField] private bool m_isDashing = false;
    [SerializeField] private int m_numberOfDash = 0;
    [SerializeField] private Vector3 m_targetMoveVelocity = Vector3.zero;
    [SerializeField] private bool m_isLightIncreasing;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_playerAudioSource;
    [SerializeField] private AudioClip m_jumpAudioClip;
    [SerializeField] private AudioClip m_landAudioClip;
    [SerializeField] private AudioClip m_airJumpAudioClip;
    [SerializeField] private AudioClip m_dashAudioClip;
    [Header("Light: ")]
    [SerializeField] private Slider m_lightSlider;
    [SerializeField] private Light2D m_playerLight;
    [SerializeField] private float m_maxLightIntensity;
    [SerializeField] private float m_minLightIntensity;
    [SerializeField] private float m_minLightInnerRadius;
    [SerializeField] private float m_maxLightOuterRadius;
    //[SerializeField] private float m_minLightInnerRadius;
    [SerializeField] private float m_increasingLightSpeed;
    [SerializeField] private float m_decreasingLightSpeedMedium;
    [SerializeField] private float m_decreasingLightSpeedHard;
    [SerializeField] private float m_currentLightIntensity;
    [SerializeField] private float m_currentLightOuterRadius;
    [SerializeField] private float m_lightAjustment;

    public bool FacingRight { get => m_isFacingRight; set => m_isFacingRight = value; }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_animator.SetBool("isDead", false);
        m_canWallJump = true;

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

    private void Update()
    {
        m_axisX = InputManager.instance.ReturnAxisX();
        AnimatePlayer();
        AjustPlayerLight();
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        Move(m_axisX * Time.fixedDeltaTime * m_playerMoveSpeed);
        TryToWallSlide();

        if (!m_isWallJumping)
        {
            TryToFlip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_animator.SetBool("isDead", true);
            GameManager.instance.EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            m_isLightIncreasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            m_isLightIncreasing = false;
        }
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
        // Ajust the player's light (reminder: 2 is easy, 1 is medium, 0 is hard)
        if (PlayerPrefs.GetInt("Difficulty") != 2 && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            if (m_isLightIncreasing && m_playerLight.intensity < m_maxLightIntensity)
            {
                // Increases the player's light quickly
                m_lightAjustment = Time.deltaTime * m_increasingLightSpeed;
                m_playerLight.intensity += m_lightAjustment;

                if (m_playerLight.intensity > m_maxLightIntensity)
                {
                    m_playerLight.intensity = m_maxLightIntensity;
                }
            }
            else if (!m_isLightIncreasing && m_playerLight.intensity > m_minLightIntensity)
            {
                // Decreases the player's light slowly, depending on the difficulty setting
                if (PlayerPrefs.GetInt("Difficulty") == 1)
                {
                    m_lightAjustment = Time.deltaTime * m_decreasingLightSpeedMedium;
                }
                else
                {
                    m_lightAjustment = Time.deltaTime * m_decreasingLightSpeedHard;
                }

                m_playerLight.intensity -= m_lightAjustment;

                if (m_playerLight.intensity < m_minLightIntensity)
                {
                    m_playerLight.intensity = m_minLightIntensity;
                }
            }
            else if (m_playerLight.intensity == m_minLightIntensity)
            {
                // If your light is already at it's lowest, you die
                m_animator.SetBool("isDead", true);
                GameManager.instance.EndGame();
            }

            m_currentLightOuterRadius = m_playerLight.pointLightOuterRadius;
            m_currentLightIntensity = m_playerLight.intensity;
            m_lightSlider.value = m_playerLight.intensity;
        }
    }

    private void Move(float move)
    {
        //only control the player if grounded or airControl is turned on, you're not dashing and you're playing the game
        if ((m_isGrounded || m_isAirControlAllowed) && !m_isWallJumping &&!m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_targetMoveVelocity, m_MovementSmoothing);
        }
    }

    private void UpdateGrounded()
    {
        bool wasGrounded = m_isGrounded;
        m_isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_isGrounded = true;
                m_numberOfAirJumps = m_maxNumberOfAirJumps;
                m_numberOfDash = m_maxNumberOfDash;

                if (!wasGrounded && m_isGrounded)
                {
                    m_playerAudioSource.PlayOneShot(m_landAudioClip);
                }
            }
        }
    }

    public void TryToJump()
    {
        if (m_isGrounded && !m_isDashing && !m_isWallSliding && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Jump from the ground
            m_isGrounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_jumpForce);
            Debug.Log("The player jumped!");
            PlayerPrefs.SetInt("Jumps_Count", PlayerPrefs.GetInt("Jumps_Count") + 1);
            m_playerAudioSource.PlayOneShot(m_jumpAudioClip);
        }
        else if ((m_isInfiniteAirJumpsAllowed || m_numberOfAirJumps > 0) && !m_isGrounded && !m_isDashing && !m_isWallJumping && !m_isWallSliding
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Jump from the air
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_airJumpForce);
            m_numberOfAirJumps--;
            Debug.Log("The player air-jumped!");
            PlayerPrefs.SetInt("AirJumps_Count", PlayerPrefs.GetInt("AirJumps_Count") + 1);
            m_playerAudioSource.PlayOneShot(m_airJumpAudioClip);
        }
        else if (m_canWallJump && m_isWallSliding && !m_isGrounded && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            Debug.Log("The player wall-jumped!");
            StartCoroutine(WallJump());
        }
    }

    private void OldWallJump()
    {
        if (m_isWallSliding)
        {
            m_isWallJumping = false;
            m_wallJumpingDirection = transform.localScale.x;
            m_wallJumpingCounter = m_wallJumpingTime;
        }
        else
        {
            m_wallJumpingCounter -= Time.deltaTime;
        }

        if (transform.localScale.x != m_wallJumpingDirection)
        {
            m_isFacingRight = !m_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        Invoke(nameof(StopWallJumping), m_wallJumpingDuration);
    }

    private void StopWallJumping()
    {
        m_isWallJumping = false;
    }

    private void TryToWallSlide()
    {
        if (m_isWallJumpUnlocked && IsPlayerWalled() && !m_isGrounded && m_axisX != 0)
        {
            m_isWallSliding = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -m_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            m_isWallSliding = false;
        }
    }

    public void TryToDash()
    {
        if ((m_isInfiniteDashAllowed || m_numberOfDash > 0) && m_canDash
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
            // Switch the way the player is labelled as facing.
            m_isFacingRight = !m_isFacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private IEnumerator Dash()
    {
        PlayerPrefs.SetInt("Dashes_Count", PlayerPrefs.GetInt("Dashes_Count") + 1);

        m_canDash = false;
        m_isDashing = true;
        m_animator.SetBool("isDashing", true);

        //Remove gravity for now
        float originalGravity = m_Rigidbody2D.gravityScale;
        m_Rigidbody2D.gravityScale = 0f;

        // Start dashing and make the trail appear behind you
        m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_dashForce, 0f);
        m_trailRenderer.emitting = true;
        m_playerAudioSource.PlayOneShot(m_dashAudioClip);

        //Wait for the player to finish dashing
        yield return new WaitForSeconds(m_dashDuration);
        m_trailRenderer.emitting = false;
        m_Rigidbody2D.gravityScale = originalGravity;
        m_isDashing = false;
        m_animator.SetBool("isDashing", false);

        // Wait for the cooldown to finish before allowing you to dash again
        yield return new WaitForSeconds(m_dashingCooldownTime);
        m_canDash = true;
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

        if (transform.localScale.x != m_wallJumpingDirection)
        {
            m_isFacingRight = !m_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        yield return new WaitForSeconds(m_wallJumpingDuration);

        m_canWallJump = true;
        m_isWallJumping = false;
    }

    private bool IsPlayerWalled()
    {
        return Physics2D.OverlapCircle(m_wallCheck.position, 0.2f, m_wallLayer);
    }
}