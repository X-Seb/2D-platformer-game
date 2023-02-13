using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private Direction direction;
    [Header("If direction is linear, apply a single force:")]
    [SerializeField] private float m_bounceForce;
    [Header("If direction is diagonal, apply a force on each axis:")]
    [SerializeField] private float m_bounceForceX;
    [SerializeField] private float m_bounceForceY;



    enum Direction
    {
        up,
        down,
        left,
        right,
        up_left,
        up_right,
        down_left,
        down_right
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.GetState() == GameManager.GameState.playing)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerManager.instance.PlaySound(PlayerManager.SoundType.bouncy);

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
                case Direction.up_left:
                    float force1 = m_bounceForce * 0.5f;
                    rb.velocity = new Vector2(-m_bounceForceX, m_bounceForceY);
                    break;
                case Direction.up_right:
                    float force2 = m_bounceForce * 0.5f;
                    rb.velocity = new Vector2(m_bounceForceX, m_bounceForceY);
                    break;
                case Direction.down_left:
                    float force3 = m_bounceForce * 0.5f;
                    rb.velocity = new Vector2(-m_bounceForceX, -m_bounceForceY);
                    break;
                case Direction.down_right:
                    float force4 = m_bounceForce * 0.5f;
                    rb.velocity = new Vector2(m_bounceForceX, -m_bounceForceY);
                    break;
                default:
                    break;
            }


            
        }
    }
}
