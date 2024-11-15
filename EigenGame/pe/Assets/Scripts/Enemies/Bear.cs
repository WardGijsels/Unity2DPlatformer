using UnityEngine;

public class Bear : BaseEnemy
{
    [Header("Patrol Instellingen")]
    public float patrolSpeed = 2f; // Snelheid van de beer

    public float moveDistance = 5f; // Afstand die de beer kan bewegen

    private Vector2 startingPosition; // Beginpositie van de beer
    private bool movingRight = true; // Bepaalde richting waarin de beer beweegt

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        isGrounded = IsGrounded();

        // Bepaal de richting op basis van de movingRight boolean
        float direction = movingRight ? 1f : -1f;

        // Raycast om te controleren of er grond voor de beer is
        Vector2 rayOrigin = transform.position + new Vector3(direction * 0.5f, -0.1f, 0); // Licht vooraan en iets onder
        RaycastHit2D groundInFront = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, groundLayer);

        // Beweeg de beer als hij op de grond staat en er is grond voor hem
        if (isGrounded && groundInFront.collider != null)
        {
            Move(direction); // Beweeg de beer in de juiste richting
        }
        else if (!groundInFront.collider && isGrounded)
        {
            // Als er geen grond voor de beer is, draai dan om
            Flip(new Vector2(-direction, 0)); // Draai de beer om
            movingRight = !movingRight; // Wissel de bewegingsrichting
        }
    }

    protected void Move(float direction)
    {
        // Beweeg de beer naar links of rechts op basis van de richting
        rb.velocity = new Vector2(direction * patrolSpeed, rb.velocity.y); // Stel de snelheid in
    }
}