using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;

    [Header("Target")]
    public float chaseRange;
    public float attackRange;
    private Player player;

    // Components
    private Rigidbody2D rb2;

    void Awake()
    {
        // Get the player target
        player = FindAnyObjectByType<Player>();
        // Get own rigid body
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        if (playerDistance <= attackRange)
        {
            // Attack
            rb2.linearVelocity = Vector2.zero;
        }
        else if (playerDistance <= chaseRange)
        {
            Chase();
        }
        else
        {
            rb2.linearVelocity = Vector2.zero;
        }
    }

    void Chase()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        rb2.linearVelocity = dir * moveSpeed;
    }

}
