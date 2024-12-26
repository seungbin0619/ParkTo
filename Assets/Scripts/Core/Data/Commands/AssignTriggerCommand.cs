public class AssignTriggerCommand : ICommand
{
    private readonly IAssignable<Trigger> _target;
    private readonly Trigger _trigger;

    public AssignTriggerCommand(IAssignable<Trigger> target, Trigger trigger)
    {
        _target = target;
        _trigger = trigger;
    }

    public virtual void Execute()
    {
        _target.Assign(_trigger);
    }

    public virtual void Undo()
    {
        _target.Unassign(_trigger);
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