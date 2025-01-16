using System;
using UnityEngine;

public partial class Car
{
    public CarVariables Variables { get; private set; }
    public Color Color { get; }
    public Ground Ground { get; private set; }

    public Car(CarSerializer serilizer, Ground ground) {
        Color = serilizer.color;

        Ground = ground;
        SetVariables(new CarVariables(serilizer));
    }

    // TODO: SetVariables, Move 리팩토링링
    public void SetVariables(CarVariables variables) {
        Variables = variables;

        Ground = Ground.MoveTo(variables.position);
    }

    public void Move() {
        if(!CanMove()) return;  
        Variables = Variables.Next();

        Ground?.Exit(this);
        Ground = Ground?.MoveTo(Variables.position);
        Ground?.Enter(this);

        if(!CanMove()) {
            Stop();
            return;
        }
    }

    public void Stop() {
        Variables.Stop();
    }

    public void Reset() {
        Variables.Reset();
    }

    public bool CanMove() {
        if(Variables.isStop || Variables.isBroken) return false;
        Ground next = Ground.Next(Variables.GetDirection());
        
        return next != null;
    }
}