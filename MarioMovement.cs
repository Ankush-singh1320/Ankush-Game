using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rotationSpeed = 200f;

    private Rigidbody2D rb2d;
    private Rigidbody rb3d;
    private bool isGrounded;

    private enum GameDimension { TwoD, ThreeD, TopDown }
    private GameDimension currentDimension = GameDimension.TwoD;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb3d = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleDimensionSwitch();
        HandleJump();
    }

    private void HandleMovement()
    {
        if (currentDimension == GameDimension.TwoD)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
        }
        else if (currentDimension == GameDimension.ThreeD)
        {
            float moveInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveInput, 0, verticalInput).normalized;
            rb3d.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            if (moveDirection.magnitude > 0)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (currentDimension == GameDimension.TwoD)
                rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            else if (currentDimension == GameDimension.ThreeD)
                rb3d.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDimensionSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentDimension)
            {
                case GameDimension.TwoD:
                    SwitchTo3D();
                    break;
                case GameDimension.ThreeD:
                    SwitchToTopDown();
                    break;
                case GameDimension.TopDown:
                    SwitchTo2D();
                    break;
            }
        }
    }

    private void SwitchTo2D()
    {
        currentDimension = GameDimension.TwoD;
        rb2d.isKinematic = false;
        rb3d.isKinematic = true;
    }

    private void SwitchTo3D()
    {
        currentDimension = GameDimension.ThreeD;
        rb2d.isKinematic = true;
        rb3d.isKinematic = false;
    }

    private void SwitchToTopDown()
    {
        currentDimension = GameDimension.TopDown;
        // For TopDown, you may need to handle a new physics system, e.g., restricting movement to the X and Z axes.
        rb2d.isKinematic = true;
        rb3d.isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
