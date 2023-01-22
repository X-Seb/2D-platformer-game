using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Setup: ")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;

    [Header("Movement values: ")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    [Header("Other: ")]
    [SerializeField] private Vector2 movement = new Vector2(0, 0);
    [SerializeField] private float axis;
    [SerializeField] private PlayerState startingPlayerState;

    public PlayerState currentPlayerState;

    private void Awake()
    {
        instance = this;
    }

    public enum PlayerState
    {
        idle_right,
        idle_left,
        moving_left,
        moving_right,
        air_left,
        air_right
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

    void Start()
    {
        currentPlayerState = startingPlayerState;
    }

    public void MovePlayer(float axisX)
    {
        movement = new Vector2(axisX * moveSpeed, 0);
        rb.AddForce(movement);
        Debug.Log("Move!");
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce));
        Debug.Log("Jump!");
    }

    public void Dash()
    {
        rb.AddForce(new Vector2(dashForce, 0));
        Debug.Log("Dash!");
    }
}