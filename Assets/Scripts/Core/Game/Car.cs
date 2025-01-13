using System;
using UnityEngine;

public partial class Car
{
    public CarVariables Variables { get; private set; }
    public Color Color { get; }
    private Ground _ground;

    public Car(CarSerializer serilizer, Ground ground) {
        Color = serilizer.color;

        _ground = ground;
        SetVariables(new CarVariables(serilizer));
    }

    public void SetVariables(CarVariables variables) {
        Variables = variables;
        
        _ground?.Exit(this);
        _ground = _ground?.MoveTo(variables.position);
        _ground?.Enter(this);
    }

    public void Move() {
        if(Variables.isStop) return;

        SetVariables(Variables.Next());

        if(!CanMove()) {
            Stop();
            return;
        }
    }

    private void Stop() {
        Variables.Stop();
    }

    public void Reset() {
        Variables.Reset();
    }

    public bool CanMove() {
        if(Variables.isStop || Variables.isBroken) return false;
        Ground next = _ground.Next(Variables.GetDirection());
        if(next == null) return false;

        return next.IsEnterable;
    }
}