using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private Vector2 origin;
    private Vector2 direction;
    private bool touched;
    private int pointerID;

    void Awake()
    {
        direction = Vector2.zero;
        touched = false;
        origin = transform.position;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(data.position);
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(data.position);
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public float GetDirection()
    {
        return direction.x;
    }
}
