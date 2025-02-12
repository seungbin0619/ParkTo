#pragma warning disable UNT0008

using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class GroundView : MonoBehaviour {
    private Ground _ground;
    public Point position => _ground.Position;

    public void Initialize(Ground ground) {
        _ground = ground;
    }

    void Start() {
        ApplyVisual();
    }

    void OnDrawGizmos() {
        if(!Application.isPlaying) return;
        if(_ground._enteredCars.Count == 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

public partial class GroundView : IAssignableView {
    
    void OnMouseEnter()
    {
        ViewInputModule.current?.Select(this);
    }

    void OnMouseExit()
    {
        ViewInputModule.current?.Select(null);
    }

    void OnMouseDown()
    {
        ViewInputModule.current?.OnSubmitted();
    }


    public void Assign(Trigger trigger)
    {
        _ground.SetTrigger(trigger);

        ApplyVisual();
    }

    public void Unassign(Trigger _)
    {
        _ground.SetTrigger(null);

        ApplyVisual();
    }

    public void ApplyVisual() {
        GetComponentInChildren<GroundTriggerView>().SetTrigger(_ground.Trigger?.Type ?? TriggerType.None);
    }

    public bool IsAssignable(Trigger trigger)
    {
        return !_ground.HasTrigger;
    }
}