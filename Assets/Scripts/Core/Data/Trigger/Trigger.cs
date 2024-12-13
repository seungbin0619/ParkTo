public abstract class Trigger {
    public enum Type {
        None = -1,
        TurnRight = 0,
        TurnLeft,
        Stop,
        BackUp,
    }

    public abstract Type type { get; }

    public void Assign(IAssignable<Trigger> target) {
        if(!target.IsAssignable(this)) return;
        
        target.Assign(this);
    }

    public abstract void Execute(Car car);
    // public abstract void Undo();
}