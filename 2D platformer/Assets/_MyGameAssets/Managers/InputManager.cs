using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Setup: ")]
    public static InputManager instance;
    [SerializeField] private PlayerManager m_playerManager;
    [Header("Movement: ")]
    [SerializeField] private float axisX;

    private void Awake()
    {
        instance = this;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        m_playerManager.TryToJump(ctx);
    }

    public void MovePlayer(InputAction.CallbackContext value)
    {
        axisX = value.ReadValue<float>();
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            m_playerManager.TryToDash();
        }
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameManager.instance.SwitchPause();
            Debug.Log("Pause!");
        }
    }

    public float ReturnAxisX()
    {
        return axisX;
    }
}