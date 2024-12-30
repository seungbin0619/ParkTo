using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public abstract class SelectableObject : MonoBehaviour, 
    ISelectable, 
    ISelectHandler, 
    IDeselectHandler
{
    private bool _isSelected = false;
    private Selectable _selectable;

    private void Awake() {
        _selectable = GetComponent<Selectable>();
    }

    public bool IsSelected()
    {
        return _isSelected;
    }

    public virtual void Select()
    {
        _isSelected = true;
        _selectable.Select();
    }

    public virtual void Deselect() {
        _isSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public abstract void OnDeselect(BaseEventData eventData);
    public abstract void OnSelect(BaseEventData eventData);
}