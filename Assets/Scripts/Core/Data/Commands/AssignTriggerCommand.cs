public class AssignTriggerCommand : ICommand
{
    private readonly IAssignable<Trigger> _target;
    private readonly Trigger _trigger;

    public AssignTriggerCommand(IAssignable<Trigger> target, Trigger trigger) {
        _target = target;
        _trigger = trigger;
    }

    public bool Condition() {
        return _target.IsAssignable(_trigger);
    }

    public void Execute() {
        _target.Assign(_trigger);
    }

    public void Undo() {
        _target.Unassign(_trigger);
    }
}