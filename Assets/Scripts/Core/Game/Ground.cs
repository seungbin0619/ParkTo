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

    public Ground(Grid grid, Point position, Trigger trigger = null) {
        _grid = grid;

        Position = position;
        Trigger = trigger;
    }

    public Ground(GroundSerializer serilizer) {
        Position = serilizer.position;
        Trigger = null;
    }

    public Ground Next(Direction direction) {
        return _grid[Position + direction.ToPoint()];
    }
}