using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Jumping,
    Walking
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Collider2D enemyCollider;
    [SerializeField]
    private Animator animator;

    public bool isHit;
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private Vector2 defaultPos;
    [SerializeField]
    private bool canJump;
    [SerializeField]
    private bool isDead;
    [SerializeField]
    private float movingSpeed;

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        defaultPos = transform.position;
        isDead = false;

        movingSpeed = Random.Range(1f, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            isHit = true;
            isDead = true;
            if (animator)
            {
                animator.enabled = false;
            }

            if(GameManager.instance.leftEnemies.Contains(this.gameObject))
            {
                GameManager.instance.leftEnemies.Remove(this.gameObject);
            }
            else if(GameManager.instance.rightEnemies.Contains(this.gameObject))
            {
                GameManager.instance.rightEnemies.Remove(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            StartCoroutine(WaitToJump());    
            if(isDead)
            {
                enemyCollider.enabled = false;
                rb.simulated = false;
            }
        }
    }

    IEnumerator WaitToJump()
    {
        yield return new WaitForSeconds(Random.Range(.1f, 1f));
        canJump = true;
    }

    private void FixedUpdate()
    {
        if (isHit)
        {
            RotatingEnemy();
        }

        SetEnemyBehavior(enemyType);
    }

    private void SetEnemyBehavior(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                {
                    break;
                }
            case EnemyType.Jumping:
                {
                    if (canJump && !isDead)
                    {
                        Jump();
                        canJump = false;
                    }
                    break;
                }
            case EnemyType.Walking:
                {
                    if(!isDead)
                    {
                        Walking();
                        FlipX(movingSpeed);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    private void Walking()
    {
        if (transform.position.x > defaultPos.x + 1.5f)
        {
            movingSpeed = -movingSpeed;
        }
        else if (transform.position.x < defaultPos.x - 1.5f)
        {
            movingSpeed = -movingSpeed;
        }
        rb.velocity = new Vector2(movingSpeed, 0);
    }

    private void FlipX(float x)
    {
        if (x < 0)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }
        transform.localScale = new Vector3(x, 1, 1);
    }

    private void RotatingEnemy()
    {
        if(rb.velocity.x > 0)
        {
            if (transform.rotation.z > -.7f)
            {
                transform.Rotate(0, 0, -2f);
            }
            else
            {
                isHit = false;
            }
        }
        else if(rb.velocity.x < 0)
        {
            if (transform.rotation.z < .7f)
            {
                transform.Rotate(0, 0, 2f);
            }
            else
            {
                isHit = false;
            }
        }
    }

    public void KnockBack(float directionX)
    {
        rb.velocity = Vector2.zero;
        if (directionX > 0)
        {
            rb.AddForce(new Vector2(1, 4), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-1, 4), ForceMode2D.Impulse);
        }
    }
}
