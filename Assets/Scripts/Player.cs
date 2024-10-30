using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;

    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    // Components
    private Rigidbody2D rb2;
    private SpriteRenderer sr;

    // Others
    private Vector2 facingDirection;

    void Awake()
    {
        // Get the components
        rb2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Get horizontal and vertical inputs
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");
        rb2.linearVelocity = new Vector2(xMovement, yMovement);
        rb2.linearVelocity = rb2.linearVelocity.normalized * moveSpeed;
        updateSpriteDirection(xMovement, yMovement);
    }

    // Change the position of the spirte depending on the direction you're facing
    void updateSpriteDirection(float xMovement, float yMovement)
    {
        if (xMovement > 0)
        {
            sr.sprite = rightSprite;
        }
        else if (xMovement < 0)
        {
            sr.sprite = leftSprite;
        }
        else if (yMovement > 0)
        {
            sr.sprite = upSprite;
        }
        else if (yMovement < 0)
        {
            sr.sprite = downSprite;
        }
    }
}
