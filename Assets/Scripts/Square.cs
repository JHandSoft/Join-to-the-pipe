using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject particles;
    public bool type;
    public Vector2 gravity;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    public bool entering = false;
    Vector3 desiredPos;

    void Start()
    {
        LevelsMenu.instance.AddSquare(this);
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Random.Range(90f, 180f) * (Random.value <= 0.5f ? -1 : 1);
        gravity *= Random.Range(0.3f, 0.4f);
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = type ? Container.blue : Container.yellow;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            type = !type;
            sprite.color = type ? Container.blue : Container.yellow;
        }
    }

    public void Explode()
    {
        GameObject g = Instantiate(particles, transform.position, Quaternion.identity);
        ParticleSystem.MainModule settings = g.GetComponent<ParticleSystem>().main;
        settings.startColor = type ? Container.blue : Container.yellow;
        Cam.instance.StartCoroutine(Cam.instance.Shake(6, 0.45f, 0.03f));
        Destroy(gameObject);
    }

    public void Enter(Vector3 pos)
    {
        Destroy(gameObject, 0.5f);
        Destroy(rb);
        Destroy(GetComponent<Collider2D>());
        desiredPos = pos;
        entering = true;
        LevelsMenu.instance.AddCurrent();
    }

    Vector3 Slide(Vector3 a, Vector3 b, float x)
    {
        if (Vector3.Distance(a, b) < x)
            return b;
        return a + x * (b - a).normalized;
    }

    void FixedUpdate()
    {
        if (entering)
        {
            transform.position = Slide(transform.position, desiredPos, Time.fixedDeltaTime * 2);
            return;
        }
        if (rb.velocity.magnitude < 2)
            rb.velocity += gravity * Time.fixedDeltaTime;
    }
}