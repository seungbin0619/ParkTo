using UnityEditor;
using UnityEngine;

public class LevelEditorGridView {
    private Rect rect;
    private Level level;
    private Vector2 position;
    private float cellSize = 72f;
    
    public LevelEditorGridView() {
        position = Vector2.zero;
    }

    public void Draw(Rect rect, Level level) {
        this.rect = rect;
        this.level = level;

        EditorGUI.DrawRect(this.rect, new Color32(44, 44, 44, 255));
        GUI.BeginGroup(rect);

        DrawLines();

        GUI.EndGroup();
        DrawBorder(rect, Color.gray);

        EventHandler();

        GUILayout.Label($"{position.x}, {position.y}");
    }

    private void DrawLines() {
        Vector2 target = rect.size * 0.5f; // center of canvas
        Vector2 ipos = new((int)position.x, (int)position.y);
        Vector2 dpos = position - ipos;

        Rect r = new(target - position * cellSize - Vector2.one * cellSize * 0.5f, Vector2.one * cellSize);
        EditorGUI.DrawRect(r, Color.yellow);
        GUI.Label(r, $"({0},{0})", EditorStyles.boldLabel);

        target -= dpos * cellSize;
        target -= Vector2.one * cellSize * 0.5f;

        Rect tmp = new(target, Vector2.one * cellSize);
        EditorGUI.DrawRect(tmp, Color.gray);
        GUI.Label(tmp, $"({ipos.x},{ipos.y})", EditorStyles.boldLabel);

        Rect line = new(new(target.x, 0), new(1f, rect.size.y));
        Vector2 gap = new(cellSize, 0);
        Color lineWhite = new(1, 1, 1, 0.2f);

        while(line.position.x > 0) {
            EditorGUI.DrawRect(line, lineWhite);
            line.position -= gap;
        }

        line = new(new(target.x, 0), new(1f, rect.size.y));
        while(true) {
            line.position += gap;
            if(line.position.x > rect.width) break;
            EditorGUI.DrawRect(line, lineWhite);
        }

        gap = new(0, cellSize);
        line = new(new(0, target.y), new(rect.size.x, 1f));
        while(line.position.y > 0) {
            EditorGUI.DrawRect(line, lineWhite);
            line.position -= gap;
        }
        
        line = new(new(0, target.y), new(rect.size.x, 1f));
        while(true) {
            line.position += gap;
            if(line.position.y > rect.height) break;
            EditorGUI.DrawRect(line, lineWhite);
        }
    }

    Vector2 dragOffset = Vector2.zero;
    Vector2 firstPosition = Vector2.zero;
    private void EventHandler() {
        Event e = Event.current;
        if(!rect.Contains(e.mousePosition)) return;

        if (e.type == EventType.MouseDown && e.button == 0) {
            dragOffset = e.mousePosition;
            firstPosition = position;

            e.Use();
        } else if (e.type == EventType.MouseDrag && dragOffset != Vector2.zero) {
            position = firstPosition + (e.mousePosition - dragOffset) / cellSize;
            e.Use();
        } else if (e.type == EventType.MouseUp) {
            dragOffset = firstPosition = Vector2.zero;

            e.Use();
        }
    }

    private void DrawBorder(Rect rect, Color color, float thickness = 1f) {
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        EditorGUI.DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }
}