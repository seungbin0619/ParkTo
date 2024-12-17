using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    // public CarVariables Next(int index) {
    //     var variables = new CarVariables(Variables);
    //     Ground ground = _ground;

    //     while(index-- > 0) {
    //         if(variables.isStop) return null;

    //         ground = ground.Next(variables.direction);
    //         if(ground == null) return null;

    //         variables.position = ground.Position;
    //     }

    //     return variables;
    // }
}