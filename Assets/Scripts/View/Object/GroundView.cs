using System;
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

    void OnDrawGizmos() {
        if(_ground._enteredCars.Count == 0) return;
        
        // Debug.Log(_ground.Position);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

public partial class GroundView : IAssignableView {
    public void SetFocus() {
        Debug.Log("GroundView selected!");
    }

    public void LostFocus() {
        Debug.Log("GroundView unselected!");
    }

    void OnMouseEnter()
    {
        Debug.Log("ground entered " + position);
    }

    void OnMouseExit()
    {
        Debug.Log("ground exited " + position);
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