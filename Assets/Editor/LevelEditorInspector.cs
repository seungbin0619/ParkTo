using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))] 
public class CustomEditorWithButton : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(8);
        if (GUILayout.Button("Open Level Editor", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            LevelEditorWindow.ShowWindow((Level)target);
        }
        GUILayout.Space(16);
        
        base.OnInspectorGUI();
    }
}