using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : EditorWindow
{
    private const float DefaultWidth = 700f, DefaultHeight = 500f;
    private float canvasHeight = 400f;
    private const float padding = 4f;
    private Rect canvasSize;
    private bool isDraggingLine = false;
    private float lastMouseY = 0f;
    private Level _level;
    private LevelEditorGridView gridView;

    private void OnGUI()
    {
        if (_level == null)
        {
            EditorGUILayout.HelpBox("Level 객체를 선택하세요.", MessageType.Info);
            return;
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", GUILayout.Width(72f))) {
            SaveAsset();
        }

        GUILayout.EndHorizontal();

        DrawCanvasGrid();
    }

    private void DrawCanvasGrid() {
        canvasSize = new(padding, padding + 20f, position.width - padding * 2, canvasHeight - padding * 2);
        gridView.Draw(canvasSize, _level);

        float lineY = canvasSize.yMax + 8f;
        EditorGUI.DrawRect(new Rect(canvasSize.xMin, lineY, canvasSize.width, 1f), Color.white);

        HandleHorizontalDrag(lineY);
        if (Mathf.Abs(Event.current.mousePosition.y - lineY) < 8f)
            EditorGUIUtility.AddCursorRect(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1, 1), MouseCursor.ResizeVertical);
    }

    private void HandleHorizontalDrag(float lineY) {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && Mathf.Abs(currentEvent.mousePosition.y - lineY) < 10f) {
            isDraggingLine = true;
            lastMouseY = currentEvent.mousePosition.y;
            currentEvent.Use();
        }
        else if (currentEvent.type == EventType.MouseDrag && isDraggingLine) {
            float deltaY = currentEvent.mousePosition.y - lastMouseY;
            canvasHeight = Mathf.Max(300f, canvasHeight + deltaY);
            lastMouseY = currentEvent.mousePosition.y;
            Repaint();  // 변경 사항 리페인트
            currentEvent.Use();
        }
        else if (currentEvent.type == EventType.MouseUp && isDraggingLine) {
            isDraggingLine = false;
            currentEvent.Use();
        }
    }

    // ###################################################################################

    [MenuItem("Level/Level Editor")]
    public static void ShowWindowFromMenu()
    {
        ShowWindow(); 
    }

    public static void ShowWindow(Level level = null)
    {
        var window = GetWindow<LevelEditorWindow>("Level Editor");

        window.Initialize(level);
        window.Show();
    }

    public void Initialize(Level level) {
        Rect rect = EditorGUIUtility.GetMainWindowPosition();
        Vector2 windowSize = new(DefaultWidth, DefaultHeight);
        rect.position = new(
            rect.x + (rect.width - windowSize.x) * 0.5f, 
            rect.y + (rect.height - windowSize.y) * 0.5f);
        rect.size = windowSize;

        minSize = new(rect.width * 0.5f, rect.height * 0.8f);
        position = rect;

        _level = level;
        gridView = new();
    }

    private void OnEnable()
    {
        EditorApplication.update += DetectLevelSelection;
    }

    private void OnDisable()
    {
        EditorApplication.update -= DetectLevelSelection;
        SaveAsset();
    }

    private void SaveAsset() {
        EditorUtility.SetDirty(_level);
        AssetDatabase.SaveAssets();
        Debug.Log($"Saved changes to {_level.name}");
    }

    private void DetectLevelSelection()
    {
        if (Selection.activeObject is Level selectedLevel && selectedLevel != _level)
        {
            _level = selectedLevel;
            Repaint();
        }
    }
}
