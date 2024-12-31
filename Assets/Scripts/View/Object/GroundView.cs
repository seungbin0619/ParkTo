using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class GroundView : MonoBehaviour {
    private Ground _ground;
    public Point position => _ground.Position;

    public void Initialize(Ground ground) {
        _ground = ground;
    }
}

public partial class GroundView : IAssignableView
{    public void Assign(Trigger trigger)
    {
        _ground.SetTrigger(trigger);
    }

    public void Unassign(Trigger _)
    {
        _ground.SetTrigger(null);
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !_ground.HasTrigger;
    }
}