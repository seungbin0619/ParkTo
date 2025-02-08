using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Selectable))]
public class SelectableHandler : PriorityObserver {
    private Selectable _selectable;

    protected override void Awake() {
        base.Awake();

        _selectable = GetComponent<Selectable>();
    }

    void Start() {
        SetState();
    }

    protected override void UpdateState(bool state)
    {
        _selectable.enabled = state;
    } 
}