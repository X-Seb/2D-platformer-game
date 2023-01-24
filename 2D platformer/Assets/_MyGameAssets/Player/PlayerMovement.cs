using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] BrackeysCharacterController characterController;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [Range(0.0f, 200.0f)][SerializeField] float speed;
    [SerializeField] float axisX;
    [SerializeField] bool jump = false;

    private void Update()
    {
        axisX = InputManager.instance.ReturnAxisX();
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }


    private void FixedUpdate()
    {
        characterController.Move(axisX * speed * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void Jump()
    {
        jump = true;
    }
}