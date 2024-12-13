using System;
using UnityEngine;

[Serializable]
public struct Point { 
    public int x;
    public int z;

    public Point(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public Point(Point point) {
        x = point.x;
        z = point.z;
    }

    public readonly Point Next(Direction direction) {
        return this + direction.ToPoint();
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is null) return false;
        return obj is Point point && Equals(point);
    }

    public readonly bool Equals(Point other)
    {
        return x == other.x && z == other.z;
    }

    public override readonly int GetHashCode()
    {
        unchecked { 
            return (x.GetHashCode() * 397) ^ z.GetHashCode(); 
        }
    }

    public override readonly string ToString()
    {
        return $"({x},{z})";
    }

    public static Point Zero => new(0, 0);
    public static Point One => new(1, 1);

    public static Point Up => new(0, 1);
    public static Point Down => new(0, -1);
    public static Point Left => new(-1, 0);
    public static Point Right => new(1, 0);


    public static implicit operator Vector3(Point point) {
        return new Vector3(point.x, 0, point.z);
    }

    public static implicit operator Vector2(Point point) {
        return new Vector2(point.x, point.z);
    }

    public static implicit operator Vector3Int(Point point) {
        return new Vector3Int(point.x, 0, point.z);
    }

    public static implicit operator Vector2Int(Point point) {
        return new Vector2Int(point.x, point.z);
    }

    public static Point operator +(Point p1, Point p2) {
        return new Point(p1.x + p2.x, p1.z + p2.z);
    }

    public static Point operator -(Point p1, Point p2) {
        return new Point(p1.x + p2.x, p1.z + p2.z);
    }

    public static Point operator *(Point p, int n) {
        return new Point(p.x + n, p.z + n);
    }

    public static Point operator *(int n, Point p) {
        return new Point(p.x + n, p.z + n);
    }
}