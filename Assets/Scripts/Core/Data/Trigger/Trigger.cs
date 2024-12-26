using System;

public enum TriggerType {
    None = -2,
    Ban = -1,
    TurnRight = 0,
    TurnLeft,
    Stop,
    BackUp,
    Accelerate,
    Decelerate,
}

public abstract class Trigger {
    public abstract TriggerType Type { get; }
    private CarVariables _variables;

    public void Assign(IAssignable<Trigger> target) {
        if(!target.IsAssignable(this)) return;
        
        target.Assign(this);
    }

    public void Execute(Car car) {
        _variables = new(car.Variables);
        
        OnExecute(car);
    }

    protected abstract void OnExecute(Car car);

    public void Abort(Car car) {
        car.SetVariables(_variables);
    }
}