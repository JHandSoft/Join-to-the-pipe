using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player instance;

    const float G = -25;
    const float deltaY = 1.5f;
    const float velocity = 4.5f;
    Vector2 speed = new Vector2();
    Rigidbody2D rb;
    bool pressingSpace = false;
    bool ground = false;
    bool wallL = false;
    bool wallR = false;
    bool canTurn = true;
    bool clicking = false;
    Camera cam;

    public GameObject deathEffect;
    public GameObject bullet;
    public GameObject trayectory;
    const float reloadTime = 0.4f;
    float shootCounter = 0;
    readonly GameObject[] trayectories = new GameObject[35];
    const float trayectoriesDist = 1.25f;
    AudioSource audioSource;

    [HideInInspector]
    public bool wantsJump = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Joystick.instance[1].OnRelease += Shoot;
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < trayectories.Length; i++)
            trayectories[i] = Instantiate(trayectory, transform);
    }

    public void Kill()
    {
        if (LevelsMenu.instance.winning == false)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Cam.instance.StartCoroutine(Cam.instance.Shake(6, 0.5f, 0.03f));
            LevelsMenu.instance.StartCoroutine(LevelsMenu.instance.ShowDeathMenu());
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Square") || collision.CompareTag("Container"))
            Kill();
    }

    public void OnPressSpace()
    {
        if (ground)
        {
            audioSource.Play();
            speed.y = Mathf.Sqrt(-2 * G * deltaY);
        }
        else if(wallL)
        {
            audioSource.Play();
            canTurn = false;
            speed.x = velocity;
            speed.y = Mathf.Sqrt(-2 * G * deltaY);
        }
        else if (wallR)
        {
            audioSource.Play();
            canTurn = false;
            speed.x = -velocity;
            speed.y = Mathf.Sqrt(-2 * G * deltaY);
        }
    }

    bool CheckCollision(int n)
    {
        Vector2 a = Vector2.zero;
        Vector2 b = Vector2.zero;
        switch (n)
        {
            case 0:
                a = new Vector3(-0.32f, -0.42f) + transform.position;
                b = new Vector3(0.32f, -0.32f) + transform.position;
                break;

            case -1:
                a = new Vector3(-0.42f, -0.32f) + transform.position;
                b = new Vector3(-0.32f, 0.32f) + transform.position;
                break;

            case 1:
                a = new Vector3(0.42f, 0.32f) + transform.position;
                b = new Vector3(0.32f, -0.32f) + transform.position;
                break;

            case 2:
                a = new Vector3(-0.32f, 0.42f) + transform.position;
                b = new Vector3(0.32f, 0.32f) + transform.position;
                break;
        }
        foreach (Collider2D c in Physics2D.OverlapAreaAll(a, b))
        {
            if (c.CompareTag("Ground"))
                return true;
        }
        return false;
    }

    void Shoot()
    {
        if (shootCounter >= reloadTime && instance != null)
        {
            if (!clicking)
            {
                shootCounter = 0;
                Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.forward * Joystick.instance[1].Angle * Mathf.Rad2Deg));
            }
            else
            {
                Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                float rot = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
                Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.forward * rot));
            }
        }
    }

    void FixedUpdate()
    {
        if (LevelsMenu.instance.winning)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (transform.position.y < -5)
        {
            Kill();
            return;
        }

        if (Input.GetKey(KeyCode.Escape))
            LevelsMenu.instance.Pause();

        ground = CheckCollision(0);
        wallL = CheckCollision(-1);
        wallR = CheckCollision(1);

        if (CheckCollision(2) && ground == false)
            speed.y = -3;

        if (ground || (wallL && speed.x < 0) || (wallR && speed.y > 0))
            canTurn = true;

        if (ground)
        {
            if (speed.y < 0)
                speed.y = 0;
        }
        else
            speed.y += G * Time.deltaTime;

        if (canTurn)
        {
            speed.x = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                speed.x -= velocity;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                speed.x += velocity;
            speed.x += Mathf.Clamp(Joystick.instance[0].Direction.x * velocity * 4, -velocity, velocity);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (pressingSpace == false)
                OnPressSpace();
            pressingSpace = true;
        }
        else
            pressingSpace = false;

        if (wantsJump)
        {
            wantsJump = false;
            OnPressSpace();
        }

        rb.velocity = speed;

        shootCounter += Time.fixedDeltaTime;
        if (Input.GetMouseButton(0))
        {
            if (clicking == false)
            {
                clicking = true;
                Shoot();
            }
            clicking = true;
        }
        else
            clicking = false;

        for (int i = 0; i < trayectories.Length; i++)
        {
            trayectories[i].SetActive(true);
            trayectories[i].transform.position = i * (Vector3)Joystick.instance[1].Direction.normalized * trayectoriesDist + transform.position;
        }
        for (int i = 0; i < trayectories.Length - 1; i++)
        {
            Vector2 dir = trayectories[i + 1].transform.position - trayectories[i].transform.position;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(trayectories[i].transform.position, 0.175f, dir, trayectoriesDist);
            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Square") || hit.collider.gameObject.CompareTag("Container"))
                    {
                        for (int j = i + 1; j < trayectories.Length; j++)
                            trayectories[j].SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}