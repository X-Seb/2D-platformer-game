using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static InputManager instance;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMovement playerMovement;
    [Header("Movement: ")]
    [SerializeField] private float axisX;

    private void Awake()
    {
        instance = this;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerMovement.Jump();
        }
    }

    public void MovePlayer(InputAction.CallbackContext value)
    {
        axisX = value.ReadValue<float>();
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Dash!");
        }
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Pause the game!");
        }
    }

    public void Restart(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Restart the game!");
        }
    }

    public float ReturnAxisX()
    {
        return axisX;
    }
}