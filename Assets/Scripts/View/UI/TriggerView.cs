using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerView : Selectable, ISubmitHandler
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

    public void OnSubmit(BaseEventData eventData)
    {
        _parent.OnSubmit(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        _parent.OnSubmit(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

    private void ApplyVisual() {
        // ...

        enabled = _count > 0;
    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);

        _parent.Select(Type);
    }

    public override void OnDeselect(BaseEventData eventData) {
        base.OnDeselect(eventData);

        _parent.Select(Type);
    }
}