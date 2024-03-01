using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public static Movement movement;
    public float speed = 7f;
    public float gravity = 20f;
    public float jumpHeight = 8f;
    public LayerMask groundLayer;

    private CharacterController _controller;

    public bool canMove = true;
    public Vector3 moveDir;
    public bool isGrounded;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        movement = this;
    }

    private void Update()
    {
        if (!canMove) return;
        if (isGrounded && moveDir.y <= -2)
            moveDir.y = 0;
        Vector2 input = InputManager.Instance.GetMoveDir();
        if (isGrounded)
        {
            moveDir = transform.forward * (speed * input.y) + transform.right * (speed * input.x);

            if (InputManager.Instance.jump)
            {
                // Jump
                moveDir.y = jumpHeight;
                InputManager.Instance.ResetJump();
            }
        }
        else
        {
            if (InputManager.Instance.jump) InputManager.Instance.ResetJump();
        }

        moveDir.y -= gravity * Time.deltaTime;
        _controller.Move(moveDir * Time.deltaTime);
    }

    public CharacterController GetController()
    {
        return _controller;
    }
    
}
