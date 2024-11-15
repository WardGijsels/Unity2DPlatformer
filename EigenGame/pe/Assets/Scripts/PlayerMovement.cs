using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    private Vector2 moveDirection = Vector2.zero;

    public float moveSpeed = 8f;
    private bool isFacingRight = true;
    private float speedMultiplier = 1f;

    [Header("Jumping")]
    public float jumpingPower = 12f;

    private float JumpPowerMultiplier = 1f;

    [Header("Groundcheck")]
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;

    [Header("Level handling")]
    [SerializeField] private GameController gameController;

    // Start is called before the first frame update
    private void Start()
    {
        SpeedPowerUp.onSpeedBoost += StartSpeedBoost;
        JumpPowerUp.onJumpBoost += StartJumpBoost;
    }

    // Update is called once per frame
    private void Update()
    {
        // Flip de speler gebaseerd op zijn direction
        if (moveDirection.x > 0 && !isFacingRight)
        {
            // Speler beweegt naar rechts
            Flip();
        }
        else if (moveDirection.x < 0 && isFacingRight)
        {
            // Speler beweegt naar links
            Flip();
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * speedMultiplier, rb.velocity.y);
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            return true;
        }
        return false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight; // bool uitzetten als hij niet rechts kijkt
        Vector3 localScale = transform.localScale; // De current local scale van de speler ophalen
        localScale.x *= -1; // Flip de sprite door de X-as te spiegelen
        transform.localScale = localScale; // Pas de bijgewerkte localScale toe
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded() == false)
        {
            // Als de player niet op de grond staat kan hij niet jumpen
            return;
        }

        if (context.performed)
        {
            // Jump knop wordt ingedrukt gehouden
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower * JumpPowerMultiplier);
            animator.SetTrigger("jump");
        }
        else if (context.canceled)
        {
            // Jump knop wordt ingedrukt
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f * JumpPowerMultiplier);
            animator.SetTrigger("jump");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection.x = context.ReadValue<Vector2>().x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LevelFinish"))
        {
            // gebruik de gameController om het volgende level te laden
            gameController.LoadNextLevel();
        }
    }

    private void StartJumpBoost(float JumpBoost)
    {
        StartCoroutine(JumpBoostCoroutine(JumpBoost));
    }

    private IEnumerator JumpBoostCoroutine(float JumpBoost)
    {
        JumpPowerMultiplier = JumpBoost;
        yield return new WaitForSeconds(3f);
        JumpPowerMultiplier = 1f;
    }

    private void StartSpeedBoost(float speedBoost)
    {
        StartCoroutine(SpeedBoostCoroutine(speedBoost));
    }

    private IEnumerator SpeedBoostCoroutine(float speedBoost)
    {
        speedMultiplier = speedBoost;
        yield return new WaitForSeconds(3f);
        speedMultiplier = 1f;
    }
}