using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 20f;
    public Vector2 bounceDirection = Vector2.up;

    [Header("Sound")]
    public AudioClip bounceSound;  // Drag your .wav or .mp3 sound here in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Reset vertical velocity for consistent bounce
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
            }

            // Play sound at this position
            if (bounceSound != null)
            {
                AudioSource.PlayClipAtPoint(bounceSound, transform.position);
            }
        }
    }
}
