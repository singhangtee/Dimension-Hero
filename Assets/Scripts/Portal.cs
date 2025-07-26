using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal!");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
