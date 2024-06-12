using UnityEngine;

public class Container : MonoBehaviour
{
    public static readonly Color blue = new Color(0.15f, 0.55f, 1);
    public static readonly Color yellow = new Color(0.65f, 1, 0.35f);

    public GameObject explosion;
    public GameObject square;
    public bool type;
    public float length;
    public bool[] squares;
    public float frequency;
    int current = 0;
    public float spawnCounter = 0;

    [HideInInspector]
    public bool spawning = true;
    int health = 4;
    readonly GameObject[] numbers = new GameObject[4];

    void Start()
    {
        LevelsMenu.instance.AddContainer(this);
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            switch (g.name)
            {
                case "3":
                    numbers[3] = g.gameObject;
                    break;

                case "2":
                    numbers[2] = g.gameObject;
                    break;

                case "1":
                    numbers[1] = g.gameObject;
                    break;

                case "0":
                    numbers[0] = g.gameObject;
                    break;
            }
        }
        Damage();
        if (type)
        {
            GetComponent<SpriteRenderer>().color = blue;
            foreach (GameObject g in numbers)
                g.GetComponent<SpriteRenderer>().color = new Color(blue.r / 3, blue.g / 3, blue.b / 3);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = yellow;
            foreach (GameObject g in numbers)
                g.GetComponent<SpriteRenderer>().color = new Color(yellow.r / 3, yellow.g / 3, yellow.b / 3);
        }
        foreach (GameObject g in numbers)
            g.transform.eulerAngles = Vector3.zero;
        spawnCounter = frequency;
    }

    void Damage()
    {
        health--;
        if (health <= 0)
        {
            GameObject g = Instantiate(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule settings = g.GetComponent<ParticleSystem>().main;
            settings.startColor = type ? blue : yellow;
            Destroy(gameObject);
            if (Player.instance != null)
            Player.instance.Kill();
        }

        foreach (GameObject g in numbers)
            g.SetActive(false);
        numbers[health].SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Square"))
        {
            Square s = collision.GetComponent<Square>();
            if (s.type != type)
            {
                s.Explode();
                Damage();
            }
            s.Enter(transform.position);
        }
    }

    void Spawn()
    {
        Vector2 spawnPos = transform.up * length + transform.position + Random.Range(-0.5f, 0.5f) * transform.right;
        Square s = Instantiate(square, spawnPos, Quaternion.identity).GetComponent<Square>();
        s.gravity = -transform.up;
        s.type = squares[current] ? !type : type;
        current++;
        if (current == squares.Length)
            spawning = false;
    }

    void Update()
    {
        spawnCounter += Time.deltaTime;
        if (spawnCounter >= frequency)
        {
            spawnCounter = 0;
            if (spawning)
                Spawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + transform.up * length + transform.right, transform.position + transform.up * length - transform.right);
    }
}