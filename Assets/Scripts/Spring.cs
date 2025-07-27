using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 20f;
    public Vector2 bounceDirection = Vector2.up;

    [Header("Effects (Optional)")]
    public AudioClip bounceSound;
    public ParticleSystem bounceEffect;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Optional: reset vertical velocity for consistent bounce
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);

                // Play effects
                if (bounceSound != null && audioSource != null)
                    audioSource.PlayOneShot(bounceSound);

                if (bounceEffect != null)
                    bounceEffect.Play();
            }
        }
    }
}
