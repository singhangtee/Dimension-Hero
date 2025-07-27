using UnityEngine;

public class MovingPlatformLeft : MonoBehaviour
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
        target = leftPos;  // Move left first
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == rightPos) ? leftPos : rightPos;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
