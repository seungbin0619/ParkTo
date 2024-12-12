using System;
using UnityEngine;

[Serializable]
public struct CarVariables {
    public Vector2Int position;

    [Range(0, 4)]
    public Direction direction;
    public float speed; // speed: ground per second
    public bool isStop;
    public bool isBackUp; 
    public bool isBroken;

    public void Reset() {
        speed = 1f;
        isStop = false;
        isBackUp = false;
        isBroken = false;
    }

    public void Translate(Vector2Int position) {
        this.position = position;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void Stop() {
        isStop = true;
    }

    public void BackUp() {
        isBackUp = true;
    }

    public void Broke() {
        isBroken = true;
    }
}

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

    public void Move() {
        if(Variables.isStop) return;
        if(!CanMove()) {
            Stop();
            return;
        }

        Variables.Translate(GetNextPosition());
    }

    public void Stop() {
        Variables.Stop();
    }

    public void Reset() {
        Variables.Reset();
    }

    private Vector2Int GetNextPosition() {
        return Variables.position + Variables.direction.Vector();
    }

    public bool CanMove() {
        if(Grid.GroundAt(GetNextPosition()) == null) return false;

        return true;
    }
}