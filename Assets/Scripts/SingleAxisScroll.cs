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
    [SerializeField] private int framesPerPixel = 1;

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
        for (int i = scrollSprites.Length - 1; i >= 0; i--)
        {
            if (acumulatedAnimationValue % i == 0)
            {
                myImage.sprite = scrollSprites[i];
                Debug.Log("Supposed to change to " + scrollSprites[i]);
                break;
            }
        }
        Debug.Log(acumulatedAnimationValue.ToString());
    }
}