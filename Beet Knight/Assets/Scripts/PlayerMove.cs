// Ignore Spelling: anim

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D body;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator anim;

    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isGrounded = true;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontal * speed, body.velocity.y);

        // SprawdŸ, czy postaæ jest na ziemi.
        isGrounded = IsGrounded();

        // Ustaw parametr "IsGrounded" w animatorze.
        anim.SetBool("IsGrounded", isGrounded);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) // SprawdŸ, czy postaæ jest na ziemi przed skokiem.
        {
            body.velocity = new Vector2(body.velocity.x, jumpingPower);
            anim.SetTrigger("IsJumping");
        }

        if (context.canceled && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        anim.SetBool("IsRunning", Mathf.Abs(horizontal) > 0f);
    }
}
