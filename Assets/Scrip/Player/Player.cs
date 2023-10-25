using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject achery;
    [SerializeField]
    private Trajectory trajectory;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private Rigidbody2D rb; 

    [SerializeField]
    private Vector2 mousePosition;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private float angle;

    [SerializeField]
    private GameObject currentArrow;
    [SerializeField]
    private bool isShooting = false;

    [SerializeField]
    private Vector2 workspaceVelocity;
    [SerializeField]
    private Vector2 currentVelocity;
    [SerializeField]
    private float movingSpeed = 2.5f;
    private bool isAiming = false;

    private const string IDLE = "Player";
    private const string WALK = "PlayerWalk";
    [SerializeField]
    private Sprite aimingAchery;
    [SerializeField]
    private Sprite idleAchery;
    [SerializeField]
    private GameObject hand;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trajectory.Hide();
    }

    void Update()
    {

        if (!GameManager.instance.IsGamePause() && EventSystem.current.currentSelectedGameObject == null && GameManager.instance.GetArrows() > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                trajectory.Show();
                UpdateArrowDirection();
                UpdateAcheryRotation();
                trajectory.UpdateDots(achery.transform.position, direction * 2f);
                FlipX(mousePosition);
                SetUpNewArrow();
                AimAchery();
                isAiming = true;
            }
            else if (Input.GetMouseButton(0))
            {
                UpdateArrowDirection();
                UpdateAcheryRotation();
                trajectory.UpdateDots(achery.transform.position, direction * 2f);
                FlipX(mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                trajectory.Hide();
                ArrowShoot();
                isAiming = false;
                NormalAchery();
            }
        }
        else
        {
            trajectory.Hide();
        }

        FlipPlayer();

        currentVelocity = rb.velocity;

        if(currentVelocity.x > 0.1f)
        {
            GetComponent<Animator>().Play(WALK);
        }
        else
        {
            GetComponent<Animator>().Play(IDLE);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.IsGamePause())
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).leftPosX > transform.position.x)
            {
                ChangeVelocityX(movingSpeed);
            }

            if (GameManager.instance.rightEnemies.Count <= 0)
            {
                if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).rightPosX > transform.position.x)
                {
                    ChangeVelocityX(movingSpeed);
                }
            }

            if (GameManager.instance.leftEnemies.Count <= 0 && GameManager.instance.rightEnemies.Count <= 0)
            {
                ChangeVelocityX(movingSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CompleteLevelTrigger"))
        {
            GameManager.instance.CompleteLevel();
        }
    }

    private void AimAchery()
    {
        achery.GetComponent<SpriteRenderer>().sprite = aimingAchery;

        hand.transform.localPosition = new Vector3(0, -0.1f, 0);
        hand.transform.localRotation = Quaternion.Euler(0, 0, 90);

    }

    private void NormalAchery()
    {
        achery.GetComponent<SpriteRenderer>().sprite = idleAchery;
        hand.transform.parent = this.transform;
        hand.transform.localPosition = new Vector3(-0.2f, -0.18f, 0);
        hand.transform.localRotation = Quaternion.Euler(0, 0, -45);

    }

    private void FlipPlayer()
    {
        if (!isAiming)
        {
            if (transform.position.x < LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).rightPosX)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                if(GameManager.instance.leftEnemies.Count > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    private void ChangeVelocityX(float x)
    {
        workspaceVelocity.Set(x, currentVelocity.y);
        rb.velocity = workspaceVelocity;
        currentVelocity = workspaceVelocity;
    }

    private void ArrowShoot()
    {
        if (currentArrow != null)
        {
            Rigidbody2D arrowRb = currentArrow.GetComponent<Rigidbody2D>();

            currentArrow.transform.parent = null;
            currentArrow.transform.localScale = new Vector3(1, 1, 1);
            arrowRb.isKinematic = false;
            arrowRb.AddForce(direction * 2f, ForceMode2D.Impulse);
            currentArrow.GetComponent<BoxCollider2D>().enabled = true;

            if(GameManager.instance.GetArrows() == 1)
            {
                GameManager.instance.SetLastArrow(currentArrow);
            }
            GameManager.instance.DecreseArrows();
        }
    }

    public void FlipX(Vector2 mousePos)
    {
        if(mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void UpdateArrowDirection()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        direction = mousePosition - (Vector2)achery.transform.position;
    }

    private void UpdateAcheryRotation()
    {
        if (transform.localScale.x > 0)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            achery.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            achery.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
        }

    }

    private void SetUpNewArrow()
    {
        currentArrow = ObjectPool.instance.GetPooledGameObj();
        if(currentArrow != null)
        {
            currentArrow.SetActive(true);
            currentArrow.transform.parent = achery.transform;
            currentArrow.transform.localPosition = Vector3.zero;
            currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            currentArrow.transform.localScale = new Vector3(1, 1, 1);
            currentArrow.GetComponent<Rigidbody2D>().simulated = true;
            currentArrow.GetComponent<Rigidbody2D>().isKinematic = true;
            currentArrow.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
