using UnityEngine;

public class StopTrigger : Trigger
{
    public override TriggerType Type => TriggerType.Stop;

    protected override void OnExecute(Car car) => car.Variables.Stop();
}