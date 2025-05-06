using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Range))]
public class RangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float labelWidth = 30f;
        float fieldWidth = (position.width - labelWidth * 2 - 4) / 2;

        var minLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
        var minFieldRect = new Rect(minLabelRect.xMax + 2, position.y, fieldWidth, position.height);

        var maxLabelRect = new Rect(minFieldRect.xMax + 2, position.y, labelWidth, position.height);
        var maxFieldRect = new Rect(maxLabelRect.xMax + 2, position.y, fieldWidth, position.height);

        var minProp = property.FindPropertyRelative("min");
        var maxProp = property.FindPropertyRelative("max");

        EditorGUI.LabelField(minLabelRect, "Min");
        EditorGUI.PropertyField(minFieldRect, minProp, GUIContent.none);

        EditorGUI.LabelField(maxLabelRect, "Max");
        EditorGUI.PropertyField(maxFieldRect, maxProp, GUIContent.none);

        EditorGUI.EndProperty();
    }
}
