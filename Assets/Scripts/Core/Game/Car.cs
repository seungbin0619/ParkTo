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
    }

    public void Move() {
        if(Variables.isStop) return;

        if(!CanMove()) {
            Stop();
            return;
        }

        _ground.Exit();

        Variables.position += Variables.direction.ToPoint();
        _ground = _ground.Next(Variables.direction);
        _ground.Enter();
    }

    private void Stop() {
        Variables.Stop();
    }

    public void Reset() {
        Variables.Reset();
    }

    public bool CanMove() {
        if(_ground.Next(Variables.direction) == null) return false;

        return true;
    }
}