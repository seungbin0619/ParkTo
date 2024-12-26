using UnityEngine;

public abstract class TurnTrigger : Trigger
{
    protected virtual bool IsClockwise => false;
    protected override void OnExecute(Car car) => car.Variables.Rotate(IsClockwise != car.Variables.isBackUp ? 1 : -1);
}

public class TurnRightTrigger : TurnTrigger
{
    protected override bool IsClockwise => true;
    public override TriggerType Type => TriggerType.TurnRight;
}

public class TurnLeftTrigger : TurnTrigger
{
    public override TriggerType Type => TriggerType.TurnLeft;
}