using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, speed);
        
        // На всякий случай: пуля самоуничтожится через 5 секунд
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Если пуля попала в комету - уничтожаем пулю
        if (other.CompareTag("Comet"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}