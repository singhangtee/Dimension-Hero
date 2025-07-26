using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RealityDeathChecker : MonoBehaviour {
    [Tooltip("Layers that will kill the player if overlapped after reality shift")]
    public LayerMask deathLayers;

    private Collider2D playerCollider;

    void Awake() {
        playerCollider = GetComponent<Collider2D>();
    }

    void OnEnable() {
        RealityManager.OnRealitySwitched += CheckForOverlap;
    }

    void OnDisable() {
        RealityManager.OnRealitySwitched -= CheckForOverlap;
    }

    void CheckForOverlap() {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f,
            deathLayers
        );

        foreach (var hit in hits) {
            if (hit != playerCollider) {
                KillPlayer();
                return;
            }
        }
    }

    void KillPlayer() {
        Debug.Log("Player died due to overlapping after switch.");
        Destroy(gameObject); // Replace with respawn or game over logic
    }

    void OnDrawGizmosSelected() {
        if (playerCollider == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerCollider.bounds.center, playerCollider.bounds.size);
    }
}
