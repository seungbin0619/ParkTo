public abstract class Trigger {
    public abstract TriggerType Type { get; }
    public void Assign(IAssignable<Trigger> target) {
        if(!target.IsAssignable(this)) return;
        
        target.Assign(this);
    }

    public abstract void Execute(Car car);
    // public abstract void Undo();
}