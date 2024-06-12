using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public GameObject squareA;
    //float speedA;
    //float gA;
    //float rotA;

    //public GameObject squareB;
    //float speedB;
    //float gB;
    //float rotB;

    //public GameObject player;
    //float t = -1;
    //float deltaT = 1;
    //float playerSpeed = 3;
    //const float G = -20;
    //const float deltaY = 1;

    public void LoadLevels()
    {
        SceneManager.LoadScene(1);
    }

    public void Close()
    {
        Application.Quit();
    }

    //void ReInit(bool a)
    //{
    //    if (a)
    //    {
    //        squareA.transform.position += Vector3.up * 8;
    //        speedA = 0;
    //        gA = Random.Range(-0.2f, -0.5f);
    //        rotA = Random.Range(90f, 180f) * (Random.value < 0.5f ? -1 : 1);
    //    }
    //    else
    //    {
    //        squareB.transform.position += Vector3.up * 8;
    //        speedB = 0;
    //        gB = Random.Range(-0.2f, -0.5f);
    //        rotB = Random.Range(90f, 180f) * (Random.value < 0.5f ? -1 : 1);
    //    }
    //}

    //float PlayerHeigth(float t)
    //{
    //    return Mathf.Max(-3.3f, G * t * t / 2 + t * Mathf.Sqrt(-2 * G * deltaY) - 3.3f);
    //}

    //private void FixedUpdate()
    //{
    //    speedA += gA * Time.fixedDeltaTime;
    //    squareA.transform.position += Vector3.up * speedA * Time.fixedDeltaTime;
    //    squareA.transform.eulerAngles += Vector3.forward * rotA * Time.fixedDeltaTime;
    //    if (squareA.transform.position.y < -2)
    //        ReInit(true);

    //    speedB += gB * Time.fixedDeltaTime;
    //    squareB.transform.position += Vector3.up * speedB * Time.fixedDeltaTime;
    //    squareB.transform.eulerAngles += Vector3.forward * rotB * Time.fixedDeltaTime;
    //    if (squareB.transform.position.y < -2)
    //        ReInit(false);

    //    if (player.transform.position.x >= 3)
    //        playerSpeed = -Mathf.Abs(playerSpeed);
    //    else if (player.transform.position.x <= -3)
    //        playerSpeed = Mathf.Abs(playerSpeed);
    //    player.transform.position += Vector3.right * playerSpeed * Time.fixedDeltaTime;

    //    if (t < -0.5f)
    //        deltaT = Mathf.Abs(deltaT);
    //    else if (t > 1.14f)
    //        deltaT = -Mathf.Abs(deltaT);
    //    t += Time.fixedDeltaTime * deltaT;
    //    player.transform.position = new Vector3(player.transform.position.x, PlayerHeigth(t), player.transform.position.z);
    //}
}