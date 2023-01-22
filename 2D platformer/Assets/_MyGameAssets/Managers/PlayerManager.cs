using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Setup: ")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject spriteRenderer;

    [Header("Movement values: ")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float doubleJumpForce;
    [Header("Other: ")]
    [SerializeField] private Vector2 movement = new Vector2(0, 0);
    [SerializeField] private float axis;
    [SerializeField] private PlayerState startingPlayerState;
    [SerializeField] private bool facingRight = true;
    [SerializeField] private float distToGround;
    [SerializeField] private bool grounded = false;
    [SerializeField] private int jumpsLeft = 0;
    [SerializeField] private float runningSpeed = 10;

    public PlayerState currentPlayerState;

    private void Awake()
    {
        instance = this;
    }

    public enum PlayerState
    {
        idle,
        walk,
        run,
        fall,
        jump,
        doubleJump
    }

    void Start()
    {
        currentPlayerState = startingPlayerState;
        StartAnim(currentPlayerState);
    }

    private void Update()
    {
        //Always make sure the player is facing the right direction
        if (rb.velocity.x >= 0)
        {
            facingRight = true;
            spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            facingRight = false;
            spriteRenderer.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        //Set the correct player state depending on your speed, if you're on the ground
        if (grounded)
        {
            if (rb.velocity.x > runningSpeed || rb.velocity.x < -runningSpeed)
            {
                if (currentPlayerState != PlayerState.run)
                {
                    SetState(PlayerState.run);
                    StartAnim(currentPlayerState);
                }
            }
            else if (rb.velocity.x != 0 && (rb.velocity.x < runningSpeed || -runningSpeed < rb.velocity.x))
            {
                if (currentPlayerState != PlayerState.walk)
                {
                    SetState(PlayerState.walk);
                    StartAnim(currentPlayerState);
                }
            }
            else
            {
                SetState(PlayerState.idle);
                StartAnim(currentPlayerState);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            jumpsLeft = 2;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            grounded = false;
        }
    }

    public void SetState(PlayerState newState)
    {
        currentPlayerState = newState;
        Debug.Log("The current player state is now: " + newState);
    }

    public PlayerState GetState()
    {
        return currentPlayerState;
    }

    public void MovePlayer(float axisX)
    {
        movement = new Vector2(axisX * moveSpeed, 0);
        rb.AddForce(movement);
        Debug.Log("Move!");
    }

    public void Jump()
    {
        if (jumpsLeft == 2)
        {
            SetState(PlayerState.jump);
            StartAnim(currentPlayerState);

            rb.AddForce(new Vector2(0, jumpForce));
            jumpsLeft--;
            Debug.Log("Jump!");
        }
        else if (jumpsLeft == 1)
        {
            SetState(PlayerState.doubleJump);
            StartAnim(currentPlayerState);

            rb.AddForce(new Vector2(0, doubleJumpForce));
            jumpsLeft--;
        }
    }

    public void Dash()
    {
        if (facingRight == true)
        {
            rb.AddForce(new Vector2(dashForce, 0));
            Debug.Log("Dash to the right!");
        }
        else
        {
            rb.AddForce(new Vector2(-dashForce, 0));
            Debug.Log("Dash to the right!");
        }
    }

    private bool IsPlayerGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    private void StartAnim(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.idle:
                animator.SetTrigger("idle");
                break;
            case PlayerState.walk:
                animator.SetTrigger("walk");
                break;
            case PlayerState.run:
                animator.SetTrigger("run");
                break;
            case PlayerState.jump:
                animator.SetTrigger("jump");
                break;
            case PlayerState.fall:
                animator.SetTrigger("fall");
                break;
            case PlayerState.doubleJump:
                animator.SetTrigger("doubleJump");
                break;
            default:
                animator.SetTrigger("idle");
                break;
        }
    }
}