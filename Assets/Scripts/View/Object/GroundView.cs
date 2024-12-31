using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class GroundView : MonoBehaviour, IAssignable<Trigger>, IView
{
    public Ground Ground { get; private set; }
    public Point position => Ground.Position;

    public void Initialize(Ground ground) {
        Ground = ground;
    }

    public void Assign(Trigger trigger)
    {
        Ground.SetTrigger(trigger);
    }

    public void Unassign(Trigger _)
    {
        Ground.SetTrigger(null);
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !Ground.HasTrigger;
    }
}