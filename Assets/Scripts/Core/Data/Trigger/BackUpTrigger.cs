using UnityEngine;

public class BackUpTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Stop;

    protected override void OnExecute(Car car) => car.Variables.BackUp();
}