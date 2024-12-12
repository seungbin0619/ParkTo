using UnityEngine;

public class Grid {
    public Vector2Int Size { get; }
    private Ground[,] _grounds { get; }

    public Grid(Vector2Int size) {
        Size = size;

        _grounds = new Ground[size.y, size.x];

    }

    public Ground GroundAt(Vector2Int position) {
        if(!IsValidPosition(position)) return null;
        return _grounds[position.y, position.x];
    }

    private bool IsValidPosition(Vector2Int position) {
        return position.x >= 0 && position.y >= 0 && position.x < Size.x && position.y < Size.y;
    }
}