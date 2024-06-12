using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public GameObject[] buttons;

    void ChangeIcon(GameObject button, bool unlocked)
    {
        foreach (Transform t in button.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Number")
                t.gameObject.SetActive(unlocked);
            if (t.name == "Lock")
                t.gameObject.SetActive(!unlocked);
        }
    }

    void Start()
    {
        int levels = SaveSystem.instance.GetInt("LevelsCompleted", 0);
        for (int i = 0; i < buttons.Length; i++)
            ChangeIcon(buttons[i], i <= levels);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int level)
    {
        if (SaveSystem.instance.GetInt("LevelsCompleted", 0) >= level - 1)
            SceneManager.LoadScene(level + 1);
    }
}