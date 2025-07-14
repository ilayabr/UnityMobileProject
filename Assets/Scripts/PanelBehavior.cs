using UnityEngine;
using System.Collections;

public class PanelBehavior : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private RectTransform panel;
    [SerializeField] private float speed = 100f;
    [SerializeField] private Vector2 activePosition;
    [SerializeField] private Vector2 inactivePosition;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        isActive = false;
        panel.anchoredPosition = inactivePosition;
    }

    public void OnButtonPress()
    {
        isActive = !isActive;
        Vector2 targetPosition = isActive ? activePosition : inactivePosition;
        StartCoroutine(MovePanel(targetPosition));
    }

    private IEnumerator MovePanel(Vector2 targetPosition)
    {
        Vector2 start = panel.anchoredPosition;
        float distance = Vector2.Distance(start, targetPosition);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curveT = moveCurve.Evaluate(t);
            panel.anchoredPosition = Vector2.Lerp(start, targetPosition, curveT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition;
    }
}