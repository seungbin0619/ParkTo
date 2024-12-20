using UnityEngine;

public static class TriggerGenerator
{
    public static Trigger Generate(TriggerType type)
    {
        return type switch
        {
            TriggerType.TurnRight => new TurnRightTrigger(),
            TriggerType.TurnLeft => new TurnLeftTrigger(),
            TriggerType.Stop => new StopTrigger(),
            TriggerType.BackUp => new BackUpTrigger(),
            _ => null,
        };
    }
}