using UnityEngine;

public partial class GroundView : MonoBehaviour, IAssignable<Trigger>
{
    public Ground Ground { get; private set; }

    public void Initialize(Ground ground) {
        Ground = ground;
    }

    public void Assign(Trigger trigger)
    {
        Ground.SetTrigger(trigger);
    }

    public void Unassign()
    {
        Ground.SetTrigger(null);
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !Ground.HasTrigger;
    }
}