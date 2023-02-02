using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private TrailRenderer m_trailRenderer;
    [SerializeField] private Animator m_animator;
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;  // A position marking where to check if the player is grounded.
    [Header("Input: ")]
    [SerializeField] private float m_axisX = 0f; // Either -1, 0, or 1
    [Header("Movement: ")]
    [Range(0, 200)][SerializeField] private float m_playerMoveSpeed = 80f; // How fast the player can move.
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false; // If the player can move horizontally while in the air.
    [Header("Jumping: ")]
    [Range(0, 50)][SerializeField] private float m_JumpForce = 25f; // Amount of force added when the player jumps.
    [Range(0, 50)][SerializeField] private float m_airJumpForce = 15f; // Amount of force added when the player jumps in the air.
    [SerializeField] private bool m_infiniteAirJumps = false;
    [Range(0, 10)][SerializeField] private int m_maxNumberOfAirJumps = 1;
    [Header("Dashing: ")]
    [SerializeField] private bool m_infiniteDash = false;
    [Range(0, 10)][SerializeField] private int m_maxNumberOfDash = 1;
    [Range(0, 50)][SerializeField] private float m_dashForce = 25f;
    [Range(0, 2)][SerializeField] private float m_dashTime = 0.2f;
    [Range(0, 10)][SerializeField] private float m_dashingCooldownTime = 1.5f;
    [Header("Useful information for reference only: ")]
    [SerializeField] const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private bool m_Grounded;  // Whether or not the player is grounded.
    [SerializeField] private bool m_FacingRight = true; // For determining which way the player is currently facing.
    [SerializeField] private int m_numberOfAirJumps = 0;
    [SerializeField] private bool m_canDash = true;
    [SerializeField] private bool m_isDashing = false;
    [SerializeField] private int m_numberOfDash = 0;
    [SerializeField] private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private bool m_isLightIncreasing;
    [Header("Audio: ")]
    [SerializeField] private AudioSource m_playerAudioSource;
    [SerializeField] private AudioClip m_jumpAudioClip;
    [SerializeField] private AudioClip m_landAudioClip;
    [SerializeField] private AudioClip m_airJumpAudioClip;
    [SerializeField] private AudioClip m_dashAudioClip;
    [Header("Light: ")]
    //[SerializeField] private Light2D m_playerLight;
    [SerializeField] private float m_maxLightIntensity;
    [SerializeField] private float m_minLightIntensity;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_animator.SetBool("isDead", false);

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
        m_animator.SetFloat("x-speed", Mathf.Abs(m_Rigidbody2D.velocity.x));
        m_animator.SetFloat("y-velocity", m_Rigidbody2D.velocity.y);
        m_animator.SetBool("isGrounded", m_Grounded);
        m_animator.SetFloat("x-input", m_axisX);

        // Ajust the player's light
        if (m_isLightIncreasing) // Make sure the light isn't at max power 
        {
            // Increse the light quickly to max power
        }
        else if(!m_isLightIncreasing) // Make sure your light isn't below the lowest point already
        {
            // Decrease the light slowly, depending on the difficulty setting
        }
        else if (false)
        {
            // If your light is already at it's lowest, you die
        }
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        Move(m_axisX * Time.fixedDeltaTime * m_playerMoveSpeed);
        TryToFlip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_animator.SetBool("isDead", true);
            GameManager.instance.EndGame();
        }
    }

    private void Move(float move)
    {
        //only control the player if grounded or airControl is turned on, you're not dashing and you're playing the game
        if ((m_Grounded || m_AirControl) && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
    }

    private void UpdateGrounded()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                m_numberOfAirJumps = m_maxNumberOfAirJumps;
                m_numberOfDash = m_maxNumberOfDash;

                if (!wasGrounded && m_Grounded)
                {
                    m_playerAudioSource.PlayOneShot(m_landAudioClip);
                }
            }
        }
    }

    public void TryToJump()
    {
        if (m_Grounded && !m_isDashing && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Jump from the ground
            m_Grounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
            Debug.Log("The player jumped!");
            PlayerPrefs.SetInt("Jumps_Count", PlayerPrefs.GetInt("Jumps_Count") + 1);
            m_playerAudioSource.PlayOneShot(m_jumpAudioClip);
        }
        else if ((m_infiniteAirJumps || m_numberOfAirJumps > 0) && !m_Grounded && !m_isDashing
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            // Jump from the air
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_airJumpForce);
            m_numberOfAirJumps--;
            Debug.Log("The player air-jumped!");
            PlayerPrefs.SetInt("AirJumps_Count", PlayerPrefs.GetInt("AirJumps_Count") + 1);
            m_playerAudioSource.PlayOneShot(m_airJumpAudioClip);
        }
    }

    public void TryToDash()
    {
        if ((m_infiniteDash || m_numberOfDash > 0) && m_canDash
            && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            StartCoroutine(Dash());
            m_numberOfDash--;
        }
    }

    private void TryToFlip()
    {
        // Only flip if you're facing the opposite direction that you're tring to move in.
        if ((m_FacingRight && m_axisX < 0f) || (!m_FacingRight && m_axisX > 0f))
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

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
        yield return new WaitForSeconds(m_dashTime);
        m_trailRenderer.emitting = false;
        m_Rigidbody2D.gravityScale = originalGravity;
        m_isDashing = false;
        m_animator.SetBool("isDashing", false);

        // Wait for the cooldown to finish before allowing you to dash again
        yield return new WaitForSeconds(m_dashingCooldownTime);
        m_canDash = true;
    }
}