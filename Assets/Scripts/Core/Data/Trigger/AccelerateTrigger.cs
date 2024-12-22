using UnityEngine;

public class AccelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Accelerate;
    public override void Execute(Car car)
    {
        car.Variables.speed = 2f;
    }
}