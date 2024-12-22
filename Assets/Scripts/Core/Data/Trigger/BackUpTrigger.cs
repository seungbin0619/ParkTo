using UnityEngine;

public class BackUpTrigger : Trigger
{
    public override TriggerType Type => TriggerType.BackUp;
    
    public override void Execute(Car car)
    {
        car.Variables.BackUp();
    }
}