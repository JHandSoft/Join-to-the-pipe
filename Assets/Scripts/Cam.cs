using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour
{
    public static Cam instance;

    void Awake()
    {
        instance = this;
    }

    public IEnumerator Shake(int times, float magnitude, float frequency)
    {
        for (int i = 0; i < times; i++)
        {
            transform.position = magnitude * Random.insideUnitSphere - 8 * Vector3.forward;
            yield return new WaitForSeconds(frequency);
        }
    }
}