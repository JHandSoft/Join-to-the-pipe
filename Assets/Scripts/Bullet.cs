using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float speed = 12;
    public GameObject particles;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(gameObject, 4);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Container") || collision.CompareTag("Square"))
            DestroyBullet();
    }

    void DestroyBullet()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}