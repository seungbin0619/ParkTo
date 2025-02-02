#pragma warning disable UNT0008

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class GroundView : MonoBehaviour {
    private Ground _ground;
    public Point position => _ground.Position;

    public void Initialize(Ground ground) {
        _ground = ground;
    }

    void OnDrawGizmos() {
        if(_ground._enteredCars.Count == 0) return;
        
        // Debug.Log(_ground.Position);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

public partial class GroundView : IAssignableView {
    
    void OnMouseEnter()
    {
        // Debug.Log("ground entered " + position);

        //if(ViewInputModule.current == null) return;
        ViewInputModule.current?.SelectView(this);
    }

    void OnMouseExit()
    {
        // Debug.Log("ground exited " + position);
        ViewInputModule.current?.SelectView(null);
    }

    void OnMouseDown()
    {
        ViewInputModule.current?.AssignTrigger();
    }


    public void Assign(Trigger trigger)
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