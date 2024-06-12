using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelsMenu : MonoBehaviour
{
    public static LevelsMenu instance;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;
    public bool winning = false;

    readonly List<Container> containers = new List<Container>();
    readonly List<Square> squares = new List<Square>();

    RectTransform progressRect;
    Text progressText;

    int totals = 0;
    bool hasCounted = false;
    int current = 0;

    public void AddContainer(Container item)
    {
        containers.Add(item);
    }

    public void AddSquare(Square item)
    {
        squares.Add(item);
    }

    public void AddCurrent()
    {
        current++;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        progressRect = GameObject.Find("Slider").GetComponent<RectTransform>();
        progressText = GameObject.Find("Progress").GetComponent<Text>();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void LoadNext()
    {
        try { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
        catch { LoadMenu(); }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public IEnumerator ShowDeathMenu()
    {
        yield return new WaitForSeconds(1);
        deathMenu.SetActive(true);
    }

    IEnumerator Win()
    {
        if (Player.instance == null)
            yield break;
        winning = true;
        yield return new WaitForSeconds(1);
        winMenu.SetActive(true);
        int level = SceneManager.GetActiveScene().buildIndex - 1;
        if (SaveSystem.instance.GetInt("LevelsCompleted", 0) < level)
        {
            SaveSystem.instance.SetValue("LevelsCompleted", level);
            SaveSystem.instance.SaveFile();
        }    
    }

    void UpdatePercentage()
    {
        if (hasCounted == false)
        {
            for (int i = 0; i < containers.Count; i++)
                totals += containers[i].squares.Length;
            if (totals > 0)
                hasCounted = true;
        }
        if (totals > 0)
        {
            progressText.text = (100 * current / totals) + "%";
            progressRect.localScale = new Vector3(4.5f * current / totals, 1, 1);
        }
    }

    void Update()
    {
        UpdatePercentage();
        for (int i = containers.Count - 1; i >= 0; i--)
        {
            if (containers[i].spawning == false)
                containers.RemoveAt(i);
        }

        for (int i = squares.Count - 1; i >= 0; i--)
        {
            if (squares[i] == null)
                squares.RemoveAt(i);
        }

        if (containers.Count == 0 && squares.Count == 0)
            StartCoroutine(Win());
    }
}