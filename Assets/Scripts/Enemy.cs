using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int maxHP = 100;
    public int currentHP;
    public int damage;
    public int xpToGive;

    [Header("Target")]
    public float chaseRange;
    public float attackRate;
    private float lastAttackTime;
    public float attackRange;
    private Player player;

    // Components
    private Rigidbody2D rb2;
    public Transform spawnPoint;
    private UnityEngine.Object enemyRef;
    private UnityEngine.Object item;


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
        player.AddXp(xpToGive);
        gameObject.SetActive(false);
        SpawnItem();
        Invoke("Respawn", 2f);
    }

    void SpawnItem()
    {
        item = Resources.Load("Item");
        List<Vector3> randomDropPosition = new List<Vector3>
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        };
        Instantiate(item, transform.position + randomDropPosition[UnityEngine.Random.Range(0, 4)], transform.rotation);
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
