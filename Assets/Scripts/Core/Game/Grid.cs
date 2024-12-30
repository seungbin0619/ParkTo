using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid {
    private readonly Dictionary<Point, Ground> _grounds = new();
    
    public IEnumerable<Ground> Grounds => _grounds.Values;

    public readonly Rect Rect;

    public Ground this[Point position] => GroundAt(position);

    public Grid(IEnumerable<Ground> grounds, Rect rect) {
        _grounds.Clear();
        
        foreach(var ground in grounds) {
            ground.Initialize(this);
            _grounds.Add(ground.Position, ground);
        }

        Rect = rect;
    }

    private Ground GroundAt(Point position) {
        if(IsOutOfBounds(position)) return null;
        _grounds.TryGetValue(position, out Ground ground);
        
        return ground;
    }

    public bool IsOutOfBounds(Point position) {
        return !Rect.Contains((Vector2)position);
    }
}