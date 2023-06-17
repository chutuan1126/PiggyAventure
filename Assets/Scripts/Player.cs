using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Vector3 moveInput;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;

    private bool isJumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.ModePlayer();
    }
    public void ModePlayer()
    {
        moveInput.x = Input.GetAxis("Horizontal");

        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            isJumping = true;
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (moveInput.x > 0)
        {
            animator.SetBool("Running", true);
            sprite.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            animator.SetBool("Running", true);
            sprite.flipX = true;
        } else
        {
            animator.SetBool("Running", false);
        }

        if (isJumping == false)
        {
            animator.SetBool("Jump", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isJumping = false;
        }
    }
}
