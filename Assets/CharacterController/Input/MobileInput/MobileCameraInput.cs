using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCameraInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 TouchDist { get; private set; }

    private Vector2 PointerOld;
    private int PointerId;
    private bool Pressed;

    private const float InputSensetivity = 0.2f;

    void Update()
    {
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            TouchDist = new Vector2();
        }

        TouchDist = TouchDist * InputSensetivity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
