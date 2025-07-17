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
    [SerializeField] private Animator animator;

    
    //idk what is going on with the animator, this is the closest I could get to it working
    private void Update()
    {
        if (animator != null && Input.touchCount == 0)
        {
            animator.SetBool("isScrolling", false);
        }
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
                onScrollEvent.Invoke(sensitivity * fixedDelta);
                
                if (animator != null)
                    animator.SetBool("isScrolling", true);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Stationary:
            case TouchPhase.Canceled:
            default:
                onScrollEvent.Invoke(Vector2.zero);
                
                if (animator != null)
                    animator.SetBool("isScrolling", false);
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        foreach (var touch in Input.touches)
        {
            if (touch.position == eventData.position)
                OnScroll(touch);
            else if (animator != null)
                animator.SetBool("isScrolling", false);
        }
    }
}