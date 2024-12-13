using UnityEngine;

public partial class Ground
{
    public Point Position { get; }
    public Trigger Trigger { get; private set; } = null;
    public bool HasTrigger => Trigger != null;

    public void SetTrigger(Trigger trigger) {
        Trigger = trigger;
    }

    public void Enter(Car car) {
        Trigger?.Execute(car);
    }

    public Ground(Point position, Trigger trigger = null) {
        Position = position;
        Trigger = trigger;
    }

    public Ground(GroundSerializer serilizer) {
        Position = serilizer.position;
        Trigger = null;
    }
}