using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Setup: ")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameManager gameManager;
    [Header("Movement: ")]
    [SerializeField] private float axisX;

    private void FixedUpdate()
    {
        if (axisX != 0)
        {
            playerManager.MovePlayer(axisX);
        }
    }

    public void SwitchCurrentActionMap(string map)
    {
        playerInput.SwitchCurrentActionMap(map);
        Debug.Log(playerInput.currentActionMap);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerManager.Jump();
        }
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Dash!");
        }
    }

    public void Move(InputAction.CallbackContext value)
    {
        axisX = value.ReadValue<float>();
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
}