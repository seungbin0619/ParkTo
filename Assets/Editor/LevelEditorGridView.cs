using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public partial class LevelEditorGridView {
    private Rect rect;
    private readonly Level level;
    private Vector2 position;
    private bool selectFlag = false;
    private Vector2 selectedCell;
    private Rect viewRect;
    private Vector2 viewStandardPosition;
    private Vector2 CellSize => new(cellSize, cellSize);
    private float cellSize = 72f;

    public UnityEvent<Vector2> OnSelectionChange = new();
    
    public LevelEditorGridView(Level level) {
        this.level = level;
        position = level.grounds.Count == 0 ? Vector2.zero : level.Rect.position + (level.Rect.size - Vector2.one) * 0.5f;
    }

    public void Draw(Rect rect) {
        this.rect = rect;

        EditorGUI.DrawRect(this.rect, new Color32(44, 44, 44, 255));
        GUI.BeginGroup(rect);

        DrawLevel();
        DrawLines();
        EventHandler();

        if(preSelectFlag) HighlightCell(preSelectedCell, Color.cyan);
        if(selectFlag) HighlightCell(selectedCell, Color.white);

        GUI.EndGroup();
        DrawBorder(rect, Color.gray);

        GUILayout.Label($"{position.x}, {position.y}");
    }

    private void DrawLevel() {
        DrawGrounds();
    }

    private void DrawGrounds() {
        foreach(var ser in level.grounds) {
            Vector2 position = PositionToCell(ser.position);

            EditorGUI.DrawRect(new(position, CellSize), Color.gray);
        }
    }

    private void HighlightCell(Vector2 position, Color color) {
        DrawBorder(new(PositionToCell(position), CellSize), color, 1f);
    }

    private void DrawLines() {
        Vector2 target = rect.size * 0.5f; // center of canvas
        Vector2 ipos = new((int)position.x, (int)position.y);
        Vector2 dpos = position - ipos;

        // debugs /////////////////////////////////////////////////////////
        Rect r = new(target - position * cellSize - Vector2.one * cellSize * 0.5f, Vector2.one);
        EditorGUI.DrawRect(r, Color.red);
        ///////////////////////////////////////////////////////////////////

        target -= dpos * cellSize;
        target -= Vector2.one * cellSize * 0.5f;

        Vector2 viewSize = new((int)(target.x / cellSize), (int)(target.y / cellSize));
        Vector2 viewPosition = ipos - viewSize - Vector2.one;

        Rect vline = new(target.x - viewSize.x * cellSize, 0, 1f, rect.size.y);
        Rect hline = new(0, target.y - viewSize.y * cellSize, rect.size.x, 1f);

        viewStandardPosition.x = vline.x - cellSize;
        viewStandardPosition.y = hline.y - cellSize;
        
        viewSize = Vector2.one;
        Color lineWhite = new(1, 1, 1, 0.2f);

        while(vline.x < rect.width) {
            EditorGUI.DrawRect(vline, lineWhite);
            vline.x += cellSize;
            viewSize.x++;
        } 

        while(hline.y < rect.height) {
            EditorGUI.DrawRect(hline, lineWhite);
            hline.y += cellSize;
            viewSize.y++;
        }

        viewRect = new(viewPosition, viewSize);
    }

    private void DrawBorder(Rect rect, Color color, float thickness = 1f) {
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        EditorGUI.DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }
}

public partial class LevelEditorGridView 
{
    bool isDragging = false;
    bool preSelectFlag = false;
    Vector2 dragOrigPosition = Vector2.zero;
    Vector2 dragBeginMousePosition = Vector2.zero;
    Vector2 preSelectedCell;

    private void EventHandler() {
        Event e = Event.current;
        if(!new Rect(Vector2.zero, rect.size).Contains(e.mousePosition)) return;

        if (e.type == EventType.MouseDown) {
            OnMouseDown(e);
        } else if (e.type == EventType.MouseDrag) 
            OnMouseDrag(e);
         else if (e.type == EventType.MouseUp) {
            OnMouseUp(e);
        }

        if(e.isScrollWheel) {
            cellSize -= e.delta.y;
            cellSize = Mathf.Clamp(cellSize, 12, 216);
            e.Use();
        }
    }

    private void OnMouseDown(Event e) {
        if(e.button == 0) {
            preSelectedCell = CellToPosition(e.mousePosition);
            preSelectFlag = true;
        } else {
            isDragging = true;
            dragOrigPosition = position;
            dragBeginMousePosition = e.mousePosition;
        }

        e.Use();
    }

    private void OnMouseDrag(Event e) {
        if(e.button == 0) {
            preSelectFlag = preSelectedCell == CellToPosition(e.mousePosition);
        } else {
            if(!isDragging) return;
            position = dragOrigPosition - (e.mousePosition - dragBeginMousePosition) / cellSize;
        }

        e.Use();
    }

    private void OnMouseUp(Event e) {
        if(e.button == 0) {
            preSelectFlag = false;
            if(preSelectedCell != CellToPosition(e.mousePosition)) {
                return;
            }

            selectFlag = true;
            selectedCell = preSelectedCell;

            OnSelectionChange?.Invoke(selectedCell);
        } else {
            isDragging = false;
        }

        e.Use();
    }

    public Vector2 CellToPosition(Vector2 position) {
        Vector2 ret = viewStandardPosition - position;
        ret /= cellSize;

        ret.x = (int)ret.x;
        ret.y = (int)ret.y;

        return viewRect.position - ret;
    }

    public Vector2 PositionToCell(Vector2 position) {
        position -= viewRect.position;
        position *= cellSize;
        position += viewStandardPosition;

        return position;
    }
}