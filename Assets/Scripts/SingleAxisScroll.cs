using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleAxisScroll : MonoBehaviour, IDragHandler
{
    private static readonly int IsScrolling = Animator.StringToHash("IsScrolling");
    public UnityEvent<Vector2> onScrollEvent;
    [SerializeField] private float sensitivity = 1f; //:D
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private bool isVertical = true;
    [SerializeField] private Sprite[] scrollSprites;

    private Image myImage;
    private int acumulatedAnimationValue = 0;

    private void Start()
    {
        myImage = gameObject.GetComponent<UnityEngine.UI.Image>();
    }

    public void OnScroll(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Moved:
            case TouchPhase.Began:

                var fixedDelta = new Vector2(
                    isHorizontal ? touch.deltaPosition.x : 0f,
                    isVertical ? touch.deltaPosition.y : 0f
                );
                acumulatedAnimationValue += (int)(fixedDelta.x + fixedDelta.y);
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
        foreach (var touch in Input.touches)
        {
            if (touch.position == eventData.position)
            {
                OnScroll(touch);
                UpdateAnimationFrame();
            }
        }
    }

    private void UpdateAnimationFrame()
    {
        myImage.sprite = scrollSprites[Mathf.Abs(acumulatedAnimationValue % scrollSprites.Length)];
    }
}