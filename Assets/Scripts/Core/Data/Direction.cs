using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { 
    None, 
    Up, 
    Down, 
    Left, 
    Right,
}
public static class DirectionExtensionMethod {
    private static readonly IDictionary<Direction, Vector2Int> vectors = 
        new Dictionary<Direction, Vector2Int>() {
            { Direction.Up, Vector2Int.up },
            { Direction.Down, Vector2Int.down },
            { Direction.Left, Vector2Int.left },
            { Direction.Right, Vector2Int.right },
            { Direction.None, Vector2Int.zero }
        };

    private static readonly IDictionary<Direction, Point> points = 
        new Dictionary<Direction, Point>() {
            { Direction.Up, Point.Up },
            { Direction.Down, Point.Down },
            { Direction.Left, Point.Left },
            { Direction.Right, Point.Right },
            { Direction.None, Point.Zero }
        };

    private static readonly IDictionary<Direction, Quaternion> rotations = 
        new Dictionary<Direction, Quaternion>
        {
            { Direction.Up, Quaternion.Euler(0, 0, 0) },
            { Direction.Down, Quaternion.Euler(0, 180, 0) },
            { Direction.Left, Quaternion.Euler(0, 270, 0) },
            { Direction.Right, Quaternion.Euler(0, 90, 0) },
            { Direction.None, Quaternion.Euler(0, 0, 0) }
        };

    private static readonly IDictionary<Direction, Direction> clockwiseRotated = 
        new Dictionary<Direction, Direction>
        {
            { Direction.Up, Direction.Right },
            { Direction.Right, Direction.Down },
            { Direction.Down, Direction.Left },
            { Direction.Left, Direction.Up },
            { Direction.None, Direction.None }
        };

    private static readonly IDictionary<Direction, Direction> counterClockwiseRotated = 
        new Dictionary<Direction, Direction>
        {
            { Direction.Up, Direction.Left },
            { Direction.Left, Direction.Down },
            { Direction.Down, Direction.Right },
            { Direction.Right, Direction.Up },
            { Direction.None, Direction.None }
        };

    private static readonly IDictionary<Direction, Direction> opposites = 
        new Dictionary<Direction, Direction>() {
            { Direction.Up, Direction.Down },
            { Direction.Down, Direction.Up },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left },
            { Direction.None, Direction.None },
        };

    public static Vector2Int Vector(this Direction direction) {
        return vectors[direction];
    }

    public static Quaternion Rotation(this Direction direction) {
        return rotations[direction];
    }

    public static Direction Rotated(this Direction direction, int dir) {
        while(dir != 0) {
            if(dir > 0) {
                direction = clockwiseRotated[direction];
            } else {
                direction = counterClockwiseRotated[direction];
            }

            dir -= dir > 0 ? 1 : -1;
        }

        return direction;
    }

    public static Direction Opposite(this Direction direction) {
        return opposites[direction];
    }

    public static Point ToPoint(this Direction direction) {
        return points[direction];
    }

    public static bool IsVertical(this Direction direction) {
        return direction == Direction.Up || direction == Direction.Down;
    }

    public static bool IsHorizontal(this Direction direction) {
        return direction == Direction.Left || direction == Direction.Right;
    }

    public static bool IsPositive(this Direction direction) {
        return direction == Direction.Up || direction == Direction.Right;
    }
}