using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private float joyStickVisualDistance = 50;
    [SerializeField]
    private Image holder = null;
    [SerializeField]
    private Image joyStick = null;

    private bool dragging;
    private Vector2 direction;
    private float angle;
    private float currentAngle;

    public Vector2 Direction { get => direction; }
    public bool Dragging { get => dragging; }
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(holder.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / holder.rectTransform.sizeDelta.x);
            pos.y = (pos.y / holder.rectTransform.sizeDelta.y);

            Vector2 refPivot = new Vector2(0.5f, 0.5f);
            Vector2 tempPivot = holder.rectTransform.pivot;
            pos.x += tempPivot.x - refPivot.x;
            pos.y += tempPivot.y - refPivot.y;

            float x = Mathf.Clamp(pos.x, -1, 1);
            float y = Mathf.Clamp(pos.y, -1, 1);

            direction = new Vector2(x, y);
            dragging = true;
            angle = ((Mathf.Atan2(direction.x, direction.y) * 180.0f / Mathf.PI)) * -1;

            joyStick.rectTransform.anchoredPosition = new Vector3(x * joyStickVisualDistance, y * joyStickVisualDistance);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        direction = default(Vector3);
        joyStick.rectTransform.anchoredPosition = default(Vector3);
    }
}
