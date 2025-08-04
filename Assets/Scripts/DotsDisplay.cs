using TMPro;
using UnityEngine;

public class DotsDisplay : MonoBehaviour
{
    [SerializeField] private int Amount = 10;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private int amplitudeSensitivity = 100;
    [SerializeField] private int padding = 1;
    [SerializeField] private float sineWaveOffset = 0;
    [SerializeField] private ShootCannon cannon;
    [SerializeField] private TMP_Text text;

    private GameObject[] dots;
    private int displayLength;
    private int spacing;
    private float frequency;
    

    private void Start()
    {
        displayLength = (int)gameObject.GetComponent<RectTransform>().rect.width;
        CreateChildren();
        
        spacing = (displayLength - 2 * padding) / (Amount - 1);
        frequency = Mathf.PI * 2f / displayLength; // One full wave across the length
        
        //set initial positions
        for (int i = 0; i < dots.Length; i++)
        {
            float x = i * spacing - (displayLength - 2 * padding) / 2f;
            dots[i].transform.localPosition = new Vector2(x, 0f);
        }
    }

    private void Update()
    {
        sineWaveOffset += Time.deltaTime * amplitude * 0.05f;
        UpdateChildrenPositions();
    }

    private void CreateChildren()
    {
        if (prefab == null || Amount <= 0)
            return;

        dots = new GameObject[Amount];
        for (int i = 0; i < Amount; i++)
        {
            GameObject child = Instantiate(prefab, transform);
            dots[i] = child;
        }

        UpdateChildrenPositions();
    }

    private void UpdateChildrenPositions()
    {
        if (dots == null || Amount < 2)
            return;

        foreach (var dot in dots)
        {
            var y = Mathf.Sin(dot.transform.localPosition.x * frequency + sineWaveOffset) * amplitude;
            dot.transform.localPosition = new Vector2(dot.transform.localPosition.x, y);
        }
    }

    public void SetAmplitude(Vector2 newAmplitude)
    {
        if (newAmplitude == Vector2.zero) return;
        amplitude += newAmplitude.x * amplitudeSensitivity;
        amplitude = Mathf.Clamp(amplitude, 0f, 300f);
        cannon.cannonJammerValue = amplitude / 5;
        text.text = $"VAL...........{cannon.cannonJammerValue:F0}";
    }
}