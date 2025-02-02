using UnityEngine;

public class AssignTriggerCommand : ICommand
{
    private readonly Triggers _triggers;
    private readonly IAssignable<Trigger> _target;
    private readonly Trigger _trigger;

    public AssignTriggerCommand(Triggers triggers, IAssignable<Trigger> target, Trigger trigger) {
        Debug.Log("2");
        
        _target = target;
        _trigger = trigger;
        
        _triggers = triggers;
    }

    public bool Condition() {
        Debug.Log("3" +  _target.IsAssignable(_trigger));
        return _target.IsAssignable(_trigger);
    }

    public void Execute() {
        Debug.Log("4");

        _target.Assign(_trigger);
        _triggers.Use(_trigger);
    }

    public void Undo() {
        _target.Unassign(_trigger);
        _triggers.Cancel(_trigger);
    }
}