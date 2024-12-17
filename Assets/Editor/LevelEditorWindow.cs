using UnityEngine;
using UnityEditor;
using System.Linq;

public class LevelEditorWindow : EditorWindow
{
    public enum BrushType {
        None = -1,
        Ground,
        Car,
        Trigger,
    }

    private const float DefaultWidth = 700f, DefaultHeight = 500f;
    private float canvasHeight = 400f;
    private const float padding = 4f;
    private Rect canvasSize;
    private bool isDraggingLine = false;
    private float lastMouseY = 0f;
    private Level _level;
    private BrushType activeBrushType = BrushType.None;
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

        if (GUILayout.Toggle(activeBrushType == BrushType.Ground, new GUIContent("Ground", "Ground"), "Button"))
        {
            SetActiveBrush(BrushType.Ground);
        }

        if (GUILayout.Toggle(activeBrushType == BrushType.Car, new GUIContent("Car", "Car"), "Button"))
        {
            SetActiveBrush(BrushType.Car);
            //ShowDropdownMenu(BrushType.Car);
        }

        if (GUILayout.Toggle(activeBrushType == BrushType.Trigger, new GUIContent("Trigger", "Trigger"), "Button"))
        {
            SetActiveBrush(BrushType.Trigger);
            //ShowDropdownMenu(BrushType.Trigger);
        }

        GUILayout.EndHorizontal();

        DrawCanvasGrid();
    }

    // int selectedCarOption;
    // int selectedTriggerOption;

    // private void ShowDropdownMenu(BrushType type)
    // {
    //     GenericMenu menu = new GenericMenu();

    //     for (int i = 0; i < 4; i++)
    //     {
    //         int index = i; // Capture the index
    //         string option = i+"";

    //         if (type == BrushType.Car)
    //         {
    //             menu.AddItem(new GUIContent($"Car {option}"), selectedCarOption == i, () => 
    //             {
    //                 selectedCarOption = index;
    //                 Repaint();
    //             });
    //         }
    //         else if (type == BrushType.Trigger)
    //         {
    //             menu.AddItem(new GUIContent($"Trigger {option}"), selectedTriggerOption == i, () =>
    //             {
    //                 selectedTriggerOption = index;
    //                 Repaint();
    //             });
    //         }
    //     }

    //     menu.ShowAsContext();
    // }

    private void GridView_OnSelectionChange(Vector2 position) {
        switch(activeBrushType) {
            case BrushType.Ground:
                DrawGround(position);
                break;
            case BrushType.Car:
                
                break;
            case BrushType.Trigger:
                
                break;
            default: break;
        }
    }

    private void SetActiveBrush(BrushType type)
    {
        activeBrushType = type;
    }

    private void DrawGround(Vector2 position) {
        var ground = _level.grounds.FindAll(g => g.position == position);

        if(ground.Count == 0) {
            _level.grounds.Add(new GroundSerializer(new(position.x, position.y)));
        } else {
            _level.grounds.Remove(ground.First());
        }
    }

    private void DrawCanvasGrid() {
        canvasSize = new(padding, padding + 20f, position.width - padding * 2, canvasHeight - padding * 2);
        gridView.Draw(canvasSize);

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

        gridView?.OnSelectionChange?.RemoveAllListeners();
        gridView = new(_level);
        gridView.OnSelectionChange?.AddListener(GridView_OnSelectionChange);
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
            Initialize(selectedLevel);
            Repaint();
        }
    }
}
