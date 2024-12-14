using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(Point))]
public class EditorPointDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float labelFixedWidth = 16f;
        float fieldWidth = position.width / 2 - 2;

        Rect xRect = new(position.x + labelFixedWidth, position.y, fieldWidth - labelFixedWidth, position.height);
        Rect zRect = new(position.x + fieldWidth + 4 + labelFixedWidth, position.y, fieldWidth - labelFixedWidth, position.height);

        Rect xLabelRect = new(position.x, position.y, labelFixedWidth, position.height);
        Rect zLabelRect = new(position.x + fieldWidth + 4, position.y, labelFixedWidth, position.height);

        SerializedProperty xProp = property.FindPropertyRelative("x");
        SerializedProperty zProp = property.FindPropertyRelative("z");

        //GUIStyle labelStyle = new(GUI.skin.label) { fixedHeight = labelFixedWidth };

        EditorGUI.LabelField(xLabelRect, "X");
        EditorGUI.PropertyField(xRect, xProp, GUIContent.none);

        EditorGUI.LabelField(zLabelRect, "Z");
        EditorGUI.PropertyField(zRect, zProp, GUIContent.none);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif