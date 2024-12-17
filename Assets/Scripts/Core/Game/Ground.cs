using UnityEngine;

public partial class Ground
{
    private Grid _grid;
    public Point Position { get; }
    public Trigger Trigger { get; private set; } = null;
    public bool HasTrigger => Trigger != null;

    public void SetTrigger(Trigger trigger) {
        Trigger = trigger;
    }

    public void Enter() {
        //Trigger?.Execute(car);
    }

    public void Exit() {
        // ...
    }

    public Ground(Point position, Trigger trigger = null) {
        Position = position;
        Trigger = trigger;
    }

    public Ground(GroundSerializer serilizer) {
        Position = serilizer.position;
        Trigger = null;
    }

    public void Initialize(Grid grid) {
        _grid = grid;
    }

    public Ground Next(Direction direction) {
        return _grid[Position + direction.ToPoint()];
    }
}