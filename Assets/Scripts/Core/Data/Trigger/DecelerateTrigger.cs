using UnityEngine;

public class DecelerateTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Decelerate;
    public override void Execute(Car car)
    {
        car.Variables.speed = 0.5f;
    }
}