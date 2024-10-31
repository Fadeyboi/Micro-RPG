using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public int maxHP = 100;
    public int currentHP;
    public int damage;

    [Header("Combat")]
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    // Components
    private Rigidbody2D rb2;
    private SpriteRenderer sr;
    private Camera mainCam;
    private Vector3 mousePos;

    // Others
    private Vector2 facingDirection;

    void Awake()
    {
        // Get the components
        rb2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Move();
        updateSpriteDirection();

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackRate)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mousePos - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDirection, attackRange, 1 << 6);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy hit!");
            }
            else
            {
                Debug.LogWarning("Hit object does not have an Enemy component.");
            }
        }
    }

    void Move()
    {
        // Get horizontal and vertical inputs
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");
        rb2.linearVelocity = new Vector2(xMovement, yMovement);
        rb2.linearVelocity = rb2.linearVelocity.normalized * moveSpeed;
    }

    // Change the position of the spirte depending on the direction you're facing
    void updateSpriteDirection()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        float lookAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        if (lookAngle >= 135 || lookAngle <= -135)
        {
            sr.sprite = leftSprite;
        }
        else if (lookAngle >= 45)
        {
            sr.sprite = upSprite;
        }
        else if (lookAngle <= -45)
        {
            sr.sprite = downSprite;
        }
        else if (lookAngle <= 45 || lookAngle >= -45)
        {
            sr.sprite = rightSprite;
        }
    }

    public void TakeDamage(int damageTaken)
    {
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
