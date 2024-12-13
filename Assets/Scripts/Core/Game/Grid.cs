using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid {
    public Vector2Int Size { get; }
    private readonly Dictionary<Point, Ground> _grounds;

    public Grid(Point size) {
        Size = size;
        _grounds = new();
    }

    public Ground this[Point position] => GroundAt(position);

    public Ground GroundAt(Point position) {
        if(!IsValidPosition(position)) return null;

        return _grounds[position];
    }

    private bool IsValidPosition(Point position) {
        return _grounds.ContainsKey(position);
    }
}