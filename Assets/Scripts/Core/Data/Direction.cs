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
    private static readonly IDictionary<Direction, Vector3> vectors = 
        new Dictionary<Direction, Vector3>() {
            { Direction.Up, Vector3.up },
            { Direction.Down, Vector3.down },
            { Direction.Left, Vector3.left },
            { Direction.Right, Vector3.right },
            { Direction.None, Vector3.zero }
        };

    private static readonly IDictionary<Direction, Quaternion> rotations = 
        new Dictionary<Direction, Quaternion>
        {
            { Direction.Up, Quaternion.Euler(0, 0, 90) },
            { Direction.Down, Quaternion.Euler(0, 0, 270) },
            { Direction.Left, Quaternion.Euler(0, 0, 180) },
            { Direction.Right, Quaternion.Euler(0, 0, 0) },
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

    public static Vector3 Vector(this Direction direction) {
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

    public static bool IsVertical(this Direction direction) {
        return direction == Direction.Up || direction == Direction.Down;
    }

    public static bool IsHorizontal(this Direction direction) {
        return direction == Direction.Left || direction == Direction.Right;
    }
}