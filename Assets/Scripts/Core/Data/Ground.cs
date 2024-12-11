using UnityEngine;

public partial class Ground : MonoBehaviour, IAssignable<Trigger>
{
    public Trigger Trigger { get; private set; } = null;
    public bool HasTrigger => Trigger != null;

    public void Assign(Trigger trigger)
    {
        Trigger = trigger;
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !HasTrigger;
    }
}