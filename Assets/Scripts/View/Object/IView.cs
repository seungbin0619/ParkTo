using UnityEngine;

public interface IView {
    public Transform transform { get; }
    public Point position { get; }
}