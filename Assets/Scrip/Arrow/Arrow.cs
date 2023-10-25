using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isOutOfRange = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(rb.velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if(transform.position.x > 10f || transform.position.x < -10f)
        {
            isOutOfRange = true;
            if (isOutOfRange)
            {
                StartCoroutine(DeactiveArrow(this.gameObject));
                isOutOfRange = false;
            }

            if(this.gameObject == GameManager.instance.GetLastArrow())
            {
                GameManager.instance.Lose();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            StopArrow();
            StartCoroutine(DeactiveArrow(this.gameObject));

            if(this.gameObject == GameManager.instance.GetLastArrow())
            {
                GameManager.instance.Lose();
            } 
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().KnockBack(rb.velocity.x);
            StopArrow();
            this.transform.parent = collision.gameObject.transform;
            if (this.gameObject == GameManager.instance.GetLastArrow())
            {
                if(GameManager.instance.leftEnemies.Count + GameManager.instance.rightEnemies.Count == 0)
                {
                    GameManager.instance.Win();
                }
                else
                {
                    GameManager.instance.Lose();
                }
            }
        }

    }



    private void StopArrow()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.simulated = false;
    }

    private IEnumerator DeactiveArrow(GameObject arrow)
    {
        yield return new WaitForSeconds(3f);
        ObjectPool.instance.DeActiveObject(int.Parse(arrow.name));
    }
}
