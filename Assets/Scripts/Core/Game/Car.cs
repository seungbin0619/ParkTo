using System;
using UnityEngine;

public partial class Car
{
    public CarVariables Variables { get; private set; }
    public Color Color { get; }
    public Ground _ground;
    public bool IsStopped => Variables.isStop || Variables.isBroken;

    public Car(CarSerializer serilizer, Ground ground) {
        // Color = serilizer.color;
        _ground = ground;

        SetVariables(new CarVariables(serilizer));
    }

    // TODO: SetVariables, Move 리팩토링
    public void SetVariables(CarVariables variables) {
        Variables = variables;

        _ground?.Exit(this);
        _ground = _ground?.MoveTo(variables.position);
        _ground?.Enter(this);
    }

    public void Move() {
        SetVariables(Variables.Next());
        _ground?.Traverse(this);

        if(!CanMove()) Stop();
    }

    public void Stop() {
        Variables.Stop();
    }

    public void Broke() {
        Variables.Broke();
    }

    public void Reset() {
        Variables.Reset();
    }

    public bool CanMove() {
        if(IsStopped) return false;
        Ground next = _ground?.Next(Variables.GetDirection());
        
        return next?.IsEnterable ?? false;
    }
}