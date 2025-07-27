using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip deathSound;  // Optional sound when player dies

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Optional: Play death sound at spike's position
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            // Log and reload current scene
            Debug.Log("Player hit spike! Reloading scene...");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
