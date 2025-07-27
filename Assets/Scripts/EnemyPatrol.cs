using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrollingEnemy : MonoBehaviour
{
    public float leftOffset = -3f;
    public float rightOffset = 3f;
    public float speed = 2f;

    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 target;

    void Start()
    {
        leftPos = transform.position + new Vector3(leftOffset, 0, 0);
        rightPos = transform.position + new Vector3(rightOffset, 0, 0);
        target = leftPos;  // Start by moving left
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        Vector3 scale = transform.localScale;

        if (target.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);  // face left (default)
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // face right (flipped)
        }

        transform.localScale = scale;

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == rightPos) ? leftPos : rightPos;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

}
