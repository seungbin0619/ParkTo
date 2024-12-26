public class AccelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Accelerate;
    protected override void OnExecute(Car car) => car.Variables.speed = 2f;
}

public class DecelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Decelerate;
    protected override void OnExecute(Car car) => car.Variables.speed = 0.5f;
}