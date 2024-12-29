using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionMethods {
    private static readonly IDictionary<Vector2, Direction> directions = 
        new Dictionary<Vector2, Direction>() {
            { Vector2.down, Direction.Down },
            { Vector2.up, Direction.Up },
            { Vector2.left, Direction.Left },
            { Vector2.right, Direction.Right },
            { Vector2.zero, Direction.None }
        };

    public static Vector3 XZY(this Vector3 v) {
        (v.y, v.z) = (v.z, v.y);
        return v;
    }

    public static Vector3Int XZY(this Vector3Int v) {
        (v.y, v.z) = (v.z, v.y);
        return v;
    }

    public static Direction ToDirection(this Vector2 vector) {
        if(!directions.ContainsKey(vector)) return Direction.None;
        
        return directions[vector];
    }
}