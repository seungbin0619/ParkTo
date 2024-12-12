using UnityEngine;

public partial class GroundView : MonoBehaviour, IAssignable<Trigger>
{
    public Ground Ground { get; private set; }

    public void Assign(Trigger trigger)
    {
        Ground.SetTrigger(trigger);
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !Ground.HasTrigger;
    }
}