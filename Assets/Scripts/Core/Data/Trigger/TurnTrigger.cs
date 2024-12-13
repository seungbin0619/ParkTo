using UnityEngine;

public abstract class TurnTrigger : Trigger
{
    protected virtual bool IsClockwise => false;

    public override void Execute(Car car)
    {
        car.Variables.Rotate(IsClockwise ? 1 : -1);
    }
}

public class TurnRightTrigger : TurnTrigger
{
    public override Type type => Type.TurnRight;
}

public class TurnLeftTrigger : TurnTrigger
{
    public override Type type => Type.TurnLeft;
}