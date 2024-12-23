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

    public void Assign(IAssignable<Trigger> target) {
        if(!target.IsAssignable(this)) return;

        target.Assign(this);
    }

    public abstract void Execute(Car car);
}