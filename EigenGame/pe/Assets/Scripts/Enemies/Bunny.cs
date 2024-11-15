using UnityEngine;

public class Bunny : BaseEnemy
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    // Update is called once per frame
    private void Update()
    {
        isGrounded = IsGrounded();

        // Bereken de richting naar de speler
        Vector2 playerDirection = (player.position - transform.position).normalized;
        Flip(playerDirection);

        // Player Direction
        // Bereken de richting naar de speler en sla deze op in de variabele direction.
        // Mathf.Sign geeft 1, -1 of 0 terug afhankelijk van het teken van het verschil tussen de x-positie van de speler en de vijand.
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Player above detection
        // Controleer of de speler zich boven de vijand bevindt door een raycast naar boven uit te voeren.
        // Deze raycast heeft een lengte van 3 eenheden en kijkt specifiek naar de laag van de speler.
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);

        if (isGrounded)
        {
            Move();

            // If Ground
            // Voer een raycast uit naar voren (in de richting van de speler) om te controleren of er grond voor de vijand is.
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);

            // If gap
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);

            // If Platform above
            // Voer een raycast omhoog uit om te controleren of er een platform boven de vijand is.
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;

            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;

            // Voeg een kracht toe aan de Rigidbody2D om de vijand te laten springen.
            // ForceMode2D.Impulse zorgt ervoor dat de kracht in één keer wordt toegepast, wat geschikt is voor sprongen.
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);

            animator.SetTrigger("jump");
        }
    }
}