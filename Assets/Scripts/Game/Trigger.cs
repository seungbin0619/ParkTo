public class Trigger {
    public enum Type {
        None = -1,
        TurnRight = 0,
        TurnLeft,
        Stop,
        BackUp,
    }

    protected IAssignable<Trigger> target;
    public readonly Type type;

    public Trigger(Type type) {
        this.type = type;
    }

    public void Assign(IAssignable<Trigger> target) {
        this.target = target;

        target.Assign(this);
    }
}