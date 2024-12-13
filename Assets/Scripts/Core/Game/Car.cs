using System;
using UnityEngine;

public partial class Car
{
    public Color Color { get; }
    public Grid Grid { get; }
    public CarVariables Variables { get; private set; }

    public Car(CarVariables variables) {
        SetVariables(variables);
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
        Ground nextGround = Grid[nextPosition];

        Variables.Translate(nextPosition);
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