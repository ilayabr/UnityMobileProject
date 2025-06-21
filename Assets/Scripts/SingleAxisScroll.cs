using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleAxisScroll : MonoBehaviour, IDragHandler
{
    public UnityEvent<Vector2> onScrollEvent;
    [SerializeField] private float sensitivity = 1f; //:D
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private bool isVertical = true;

    public void OnScroll()
    {
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Moved:
            case TouchPhase.Began:
                var fixedDelta = new Vector2(
                    isHorizontal ? touch.deltaPosition.x : 0f,
                    isVertical ? touch.deltaPosition.y : 0f
                );
                onScrollEvent.Invoke(sensitivity * fixedDelta);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Stationary:
            case TouchPhase.Canceled:
            default:
                onScrollEvent.Invoke(Vector2.zero);
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnScroll();
    }
}