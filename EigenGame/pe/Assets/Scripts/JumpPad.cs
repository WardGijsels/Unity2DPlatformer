using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Pad Settings")]
    public float launchForce = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kijk of het colliding object een Rigidbody2D heeft met als tag "Player"
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Zet alle bestaande verticale snelheid op nul voordat de launchForce toegepast wordt
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0);

                // Voeg de launchForce toe op de player
                playerRb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
            }
        }
    }
}