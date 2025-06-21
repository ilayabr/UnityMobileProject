using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScopeBehavior : MonoBehaviour
{
    [SerializeField] private Transform trueScope;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Range horizontalLimit;
    [SerializeField] private Range verticalLimit;

    void Update()
    {
        //benTransform.anchoredPosition = Vector2.Lerp(benTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition, Time.deltaTime * 1f);
        trueScope.position = Vector3.MoveTowards(trueScope.position, transform.position, Time.deltaTime * speed);
    }

    public void MoveScope(Vector2 delta)
    {
        if (delta == Vector2.zero) return;

        // Calculate the new position based on the delta
        Vector3 newPosition = (Vector2)transform.position + delta;
        newPosition.x = horizontalLimit.Clamp(newPosition.x);
        newPosition.y = verticalLimit.Clamp(newPosition.y);

        // Set the new position
        transform.position = newPosition;
    }
}