using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int level;
    public Text[] texts;
    public Container container;
    public Image[] images;

    float counter = 0;

    void Update()
    {
        counter += Time.deltaTime;
        if (level == 1)
        {
            float x = Mathf.Clamp(-2 * counter + 4, 0, 1);
            for (int i = 0; i < texts.Length; i++)
            {
                float alpha = Mathf.Clamp(-Mathf.Abs(2 * counter - 8 - 12 * i) + 6, 0, 1);
                texts[i].color = new Color(1, 1, 1, alpha);
            }
            for (int i = 0; i < images.Length; i++)
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, texts[1].color.a);

            if (counter <= 24.5f)
                container.spawnCounter = 0;
        }
        else if (level == 2)
        {
            float alpha = Mathf.Clamp(-2 * counter + 4, 0, 1);
            texts[0].color = new Color(1, 1, 1, alpha);
        }
    }
}