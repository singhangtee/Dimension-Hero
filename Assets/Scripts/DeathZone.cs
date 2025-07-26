using UnityEngine;
using UnityEngine.SceneManagement; 

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the dead zone! Reloading scene...");
            // Reload the current scene to respawn the player
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
