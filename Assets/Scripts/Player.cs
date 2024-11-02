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
    public float interactRange;

    [Header("Combat")]
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    [Header("Experience")]
    public int currentLevel;
    public int currentXp;
    public int xpToNextLevel;
    public float xpModifier;

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
    private ParticleSystem[] particles;
    private ParticleSystem attackEnemyParticle;
    private ParticleSystem playerDamagedParticle;
    private PlayerUI ui;

    // Others
    private Vector2 facingDirection;

    void Awake()
    {
        // Get the components
        rb2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ui = FindAnyObjectByType<PlayerUI>();
        currentHP = maxHP;
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        attackEnemyParticle = particles[0];
        playerDamagedParticle = particles[1];
    }

    void Start()
    {
        mainCam = Camera.main;
        ui.UpdateHealthbar();
        ui.UpdateXpbar();
        ui.UpdateLevelText();
    }

    void Update()
    {
        Move();
        UpdateSpriteDirection();

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackRate)
        {
            Attack();
        }
        CheckInteract();
    }

    void CheckInteract()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mousePos - transform.position).normalized;
        RaycastHit2D interact = Physics2D.Raycast(transform.position, mouseDirection, interactRange, 1 << 7);
        if (interact.collider != null)
        {
            Interactable interactable = interact.collider.GetComponent<Interactable>();
            ui.SetInteractText(interact.collider.transform.position, interactable.interactDesc);

            if (Input.GetMouseButtonDown(1))
            {
                interactable.Interact();
            }
        }
        else
        {
            ui.DisableInteractText();
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
                // Play particle hit effect
                attackEnemyParticle.transform.position = hit.collider.transform.position;
                attackEnemyParticle.Play();

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
    void UpdateSpriteDirection()
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
        ui.UpdateHealthbar();
        playerDamagedParticle.transform.position = transform.position;
        playerDamagedParticle.Play();
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

    public void AddXp(int xp)
    {
        currentXp += xp;
        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
        else
        {
            ui.UpdateXpbar();
        }
    }

    void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;
        ui.UpdateXpbar();

        xpToNextLevel = Mathf.RoundToInt((float)xpToNextLevel * xpModifier);

        ui.UpdateLevelText();
    }
}
