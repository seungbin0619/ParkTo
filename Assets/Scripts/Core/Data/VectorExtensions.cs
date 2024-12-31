using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionMethods {
    private static readonly IDictionary<Vector2, Direction> v2directions = 
        new Dictionary<Vector2, Direction>() {
            { Vector2.down, Direction.Down },
            { Vector2.up, Direction.Up },
            { Vector2.left, Direction.Left },
            { Vector2.right, Direction.Right },
            { Vector2.zero, Direction.None }
        };

    private static readonly IDictionary<Vector3, Direction> v3directions = 
        new Dictionary<Vector3, Direction>() {
            { Vector3.down, Direction.Down },
            { Vector3.up, Direction.Up },
            { Vector3.left, Direction.Left },
            { Vector3.right, Direction.Right },
            { Vector3.zero, Direction.None }
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
        if(!v2directions.ContainsKey(vector)) return Direction.None;
        
        return v2directions[vector];
    }

    public static Direction ToDirection(this Vector3 vector) {
        vector = vector.XZY();
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);

        if(!v3directions.ContainsKey(vector)) return Direction.None;
        return v3directions[vector];
    }
}