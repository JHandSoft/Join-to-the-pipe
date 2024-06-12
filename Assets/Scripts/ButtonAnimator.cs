using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float initScale;
    float scale = 1;
    float scaleSpeed = -1;
    const float maxScale = 1.2f;

    void Awake()
    {
        initScale = transform.localScale.x;
    }

    void OnDisable()
    {
        scaleSpeed = -initScale;
        scale = initScale;
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleSpeed = Mathf.Abs(scaleSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleSpeed = -Mathf.Abs(scaleSpeed);
    }

    void Update()
    {
        scale += scaleSpeed * Time.unscaledDeltaTime;
        scale = Mathf.Clamp(scale, initScale, initScale * maxScale);
        transform.localScale = Vector3.one * scale;
    }
}