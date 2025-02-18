using UnityEngine;

public class AssignTriggerCommand : ICommand
{
    private readonly Triggers _triggers;
    private readonly IAssignable<Trigger> _target;
    private readonly Trigger _trigger;

    public AssignTriggerCommand(Triggers triggers, IAssignable<Trigger> target, Trigger trigger) {
        _target = target;
        _trigger = trigger;
        
        _triggers = triggers;
    }

    public bool Condition() {
        return _triggers.IsEnabled(_trigger.Type) && _target.IsAssignable(_trigger);
    }

    public void Execute() {
        _target.Assign(_trigger);
        _triggers.Use(_trigger);
    }

    public void Undo() {
        _target.Unassign(_trigger);
        _triggers.Cancel(_trigger);
    }
}