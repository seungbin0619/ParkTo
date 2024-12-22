using UnityEngine;

public abstract class TurnTrigger : Trigger
{
    protected virtual bool IsClockwise => false;

    public override void Execute(Car car)
    {
        // clockwise = true, car.variables.isbackup = false -> 1
        // clockwise = false, car.variables.isbackup = false -> -1
        // clockwise = true, car.variables.isbackup = true -> -1
        // clockwise = false, car.variables.isbackup = true -> 1
        
        car.Variables.Rotate(IsClockwise != car.Variables.isBackUp ? 1 : -1);
    }
}

public class TurnRightTrigger : TurnTrigger
{
    public override TriggerType Type => TriggerType.TurnRight;
    protected override bool IsClockwise => true;
}

public class TurnLeftTrigger : TurnTrigger
{
    public override TriggerType Type => TriggerType.TurnLeft;
    protected override bool IsClockwise => false;
}