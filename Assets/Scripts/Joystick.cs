using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static Joystick[] instance = new Joystick[2];

    public bool horizontal = false;
    RectTransform background;
    RectTransform handle;
    Canvas canvas;
    Camera cam;

    Vector2 dir;
    float lastAngle = 0;
    public Vector2 Direction
    {
        get { return dir; }
    }

    public float Angle
    {
        get
        {
            if (dir.magnitude > 0)
                lastAngle = Mathf.Atan2(dir.y, dir.x);
            return lastAngle;
        }
    }

    public Vector2 Pos
    {
        get { return background.transform.position; }
    }

    void Awake()
    {
        if (horizontal)
            instance[0] = this;
        else
            instance[1] = this;
    }

    void Start()
    {
        foreach (RectTransform r in GetComponentsInChildren<RectTransform>())
        {
            switch (r.gameObject.name)
            {
                case "Center":
                    handle = r;
                    break;

                case "Background":
                    background = r;
                    break;
            }
        }

        canvas = GetComponentInParent<Canvas>();
        background.pivot = Vector2.one * 0.5f;
        handle.anchorMin = Vector2.one * 0.5f;
        handle.anchorMax = Vector2.one * 0.5f;
        handle.pivot = Vector2.one * 0.5f;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
        else
            cam = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        Vector3 dir = (eventData.position - position) / (radius * canvas.scaleFactor);
        if (dir.magnitude > 1)
            dir.Normalize();
        if (horizontal)
            dir.y = 0;
        this.dir = dir;
        handle.anchoredPosition = dir * radius;
    }

    public event Action OnRelease;
    public void OnPointerUp(PointerEventData eventData)
    {
        OnRelease?.Invoke();
        dir = new Vector2();
        handle.anchoredPosition = Vector2.zero;
    }
}