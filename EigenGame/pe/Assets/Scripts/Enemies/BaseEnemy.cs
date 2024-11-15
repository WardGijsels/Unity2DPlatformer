using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Movement")]
    public Transform player;

    public float chaseSpeed = 2f;
    private bool isFacingRight = true;

    [Header("Jumping")]
    public float jumpSpeed = 2f;

    public float jumpForce = 2f;
    public bool shouldJump;

    [Header("Groundcheck")]
    public LayerMask groundLayer;

    public bool isGrounded;

    public Rigidbody2D rb;

    public int damage = 1;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Rigidbody ophalen
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the GameObject!");
        }

        // Probeer de speler te vinden als deze niet is toegewezen
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log("Player found and assigned!");
            }
            else
            {
                Debug.LogWarning("No GameObject with tag 'Player' found in the scene!");
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
    }

    // Move towards the player
    protected virtual void Move()
    {
        if (player != null && rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetVelocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
        else if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null in BaseEnemy!");
        }
    }

    protected virtual void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    public void Flip(Vector2 moveDirection)
    {
        // Controleer of de vijand naar rechts beweegt
        if (moveDirection.x > 0 && !isFacingRight)
        {
            isFacingRight = true; // De vijand kijkt nu naar rechts
            Vector3 localScale = transform.localScale; // De huidige lokale schaal van de vijand ophalen
            localScale.x = Mathf.Abs(localScale.x); // Zorg ervoor dat de schaal positief is
            transform.localScale = localScale; // Pas de bijgewerkte localScale toe
        }
        // Controleer of de vijand naar links beweegt
        else if (moveDirection.x < 0 && isFacingRight)
        {
            isFacingRight = false; // De vijand kijkt nu naar links
            Vector3 localScale = transform.localScale; // De huidige lokale schaal van de vijand ophalen
            localScale.x = -Mathf.Abs(localScale.x); // Zorg ervoor dat de schaal negatief is
            transform.localScale = localScale; // Pas de bijgewerkte localScale toe
        }
    }
}