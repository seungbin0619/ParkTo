using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Ground
{
    private Grid _grid;
    public Point Position { get; }
    public Trigger Trigger { get; private set; } = null;
    public bool HasTrigger => Trigger != null;

    ///
    
    public List<Car> _enteredCars = new();
    public bool IsEnterable => _enteredCars.All(car => car.CanMove());

    ///

    public void SetTrigger(Trigger trigger) {
        Trigger = trigger;
    }

    public void Enter(Car car) {
        _enteredCars.Add(car);
    }

    public void Traverse(Car car) {
        Trigger?.Execute(car);
    }

    public void Exit(Car car) {
        _enteredCars.Remove(car);
    }

    public Ground(Point position, Trigger trigger) {
        Position = position;
        Trigger = trigger;
    }

    public Ground(GroundSerializer serilizer) {
        Position = serilizer.position;
        
        Trigger = TriggerGenerator.Generate(serilizer.trigger);
    }

    public void Initialize(Grid grid) {
        _grid = grid;
    }

    public Ground Next(Direction direction) {
        return _grid[Position + direction.ToPoint()];
    }

    public Ground MoveTo(Point point) {
        return _grid[point];
    }
}