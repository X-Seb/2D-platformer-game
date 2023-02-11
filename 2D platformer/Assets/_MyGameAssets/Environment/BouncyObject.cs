using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private float m_bounceForce;
    [SerializeField] private Direction direction;

    enum Direction
    {
        up,
        down,
        left,
        right
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            switch (direction)
            {
                case Direction.up:
                    rb.velocity = new Vector2(rb.velocity.x, m_bounceForce);
                    break;
                case Direction.down:
                    rb.velocity = new Vector2(rb.velocity.x, -m_bounceForce);
                    break;
                case Direction.left:
                    rb.velocity = new Vector2(-m_bounceForce, rb.velocity.y);
                    break;
                case Direction.right:
                    rb.velocity = new Vector2(m_bounceForce, rb.velocity.y);
                    break;
                default:
                    break;
            }
            
        }
    }
}
