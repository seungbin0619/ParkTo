using System;
using UnityEngine;

public partial class Car
{
    public Color Color { get; }
    public Grid Grid { get; }
    public CarVariables Variables { get; private set; }

    public Car(CarSerializer serilizer, Grid grid) {
        Color = serilizer.color;
        Grid = grid;

        SetVariables(new CarVariables(serilizer));
    }

    public void SetVariables(CarVariables variables) {
        Variables = variables;
    }

    // 차는 무조건 직진만 한다.
    public void Move() {
        if(Variables.isStop) return;
        if(!CanMove()) {
            Stop();
            return;
        }

        Point nextPosition = GetNextPosition();
        Debug.Log(Variables.position + " " + nextPosition);
        Ground nextGround = Grid[nextPosition];

        Variables.Translate(Variables.direction.ToPoint());
        nextGround.Enter(this);
    }

    private void Stop() {
        Variables.Stop();
    }

    public void Reset() {
        Variables.Reset();
    }

    private Point GetNextPosition() {
        return Variables.position.Next(Variables.direction);
    }

    public bool CanMove() {
        if(Grid[GetNextPosition()] == null) return false;

        return true;
    }
}