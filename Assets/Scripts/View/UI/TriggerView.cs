using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerView : Selectable
{
    private TriggerListView _parent;

    [field: SerializeField] 
    public TriggerType Type { get; private set; } = TriggerType.None;

    private uint _count;
    public uint Count { get => _count; set {
        _count = value;
        ApplyVisual();
    }}
    
    private bool IsInfinite => Count == uint.MaxValue;

    protected override void Awake() {
        base.Awake();

        _parent = GetComponentInParent<TriggerListView>();
    }


    private void ApplyVisual() {
        // ...

        interactable = _count > 0;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        _parent.SelectTrigger(Type);
        Debug.Log(Type + " selected");
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        _parent.SelectTrigger(TriggerType.None);
        Debug.Log(Type + " deselected");
    }
}