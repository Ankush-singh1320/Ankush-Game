using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool movingRight = true;

    void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (movingRight)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            movingRight = !movingRight; // Reverse direction when hitting an obstacle
        }
    }
}
