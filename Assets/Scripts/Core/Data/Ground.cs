using UnityEngine;

public partial class Ground
{
    public Trigger Trigger { get; private set; } = null;
    public bool HasTrigger => Trigger != null;

    public void SetTrigger(Trigger trigger) {
        Trigger = trigger;
    }

    public void Enter(Car car) {
        Trigger.Execute(car);
    }
}