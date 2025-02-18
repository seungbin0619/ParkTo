public enum TriggerType {
    Goal = -2,
    Ban = -1,
    None = 0,
    TurnRight,
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