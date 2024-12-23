public class AssignTriggerCommand<T> : ICommand where T : IAssignable<Trigger>
{
    private readonly T _target;
    private readonly Trigger _trigger;

    public AssignTriggerCommand(T target, Trigger trigger)
    {
        _target = target;
        _trigger = trigger;
    }

    public virtual void Execute()
    {
        _trigger.Assign(_target);
    }

    public virtual void Undo()
    {
        _target.Unassign();
    }
}

// public class AssignTriggerToCarCommand : AssignTriggerCommand<CarView>
// {
//     private CarVariables _variables;
//     private CarView _target;

//     public AssignTriggerToCarCommand(CarView target, Trigger trigger) : base(target, trigger) { 
//         _target = target;
//     }

//     public override void Execute()
//     {
//         _variables = new(_target.Car.Variables);
//         base.Execute();
//     }

//     public override void Undo()
//     {
//         base.Undo();
//         _target.Car.SetVariables(_variables);
//     }
// }