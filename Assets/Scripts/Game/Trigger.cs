public abstract class Trigger {
    protected IAssignable<Trigger> target;
    public void Assign(IAssignable<Trigger> target) {
        this.target = target;
        
        target.Assign(this);
    }
}