using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int maxHP = 100;
    public int currentHP;
    public int damage;

    [Header("Target")]
    public float chaseRange;
    public float attackRate;
    private float lastAttackTime;
    public float attackRange;
    private Player player;

    // Components
    private Rigidbody2D rb2;
    public Transform spawnPoint;
    private Object enemyRef;


    void Awake()
    {
        // Get the player target
        player = FindAnyObjectByType<Player>();

        // Get own rigid body
        rb2 = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
    }

    void Start()
    {
        enemyRef = Resources.Load("Enemy");
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer <= attackRange)
        {
            rb2.linearVelocity = Vector2.zero;
            Hit();
        }
        else if (distanceFromPlayer <= chaseRange)
        {
            Chase();
        }
        else
        {
            rb2.linearVelocity = Vector2.zero;
        }
    }

    void Hit()
    {
        if (Time.time - lastAttackTime >= attackRate)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }

    }

    void Chase()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        rb2.linearVelocity = dir * moveSpeed;
    }



    public void TakeDamage(int damageTaken)
    {
        currentHP -= damageTaken;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", 2f);
    }

    public void Respawn()
    {
        Debug.Log("Respawning");
        if (enemyRef != null && spawnPoint != null)
        {
            Instantiate(enemyRef, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("EnemyPrefab or spawnPoint are null");
        }
        Destroy(gameObject);
    }

}
