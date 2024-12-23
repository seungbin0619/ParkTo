public class AccelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Accelerate;

    public override void Execute(Car car)
    {
        car.Variables.speed = 2f;
    }
}

public class DecelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Decelerate;

    public override void Execute(Car car)
    {
        car.Variables.speed = 0.5f;
    }
}