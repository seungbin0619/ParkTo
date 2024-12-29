using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FocusableObject : MonoBehaviour, IFocusable
{
    private bool _isFocused = false;
    private readonly List<IFocusable> children;
    private bool IsLeaf => children?.Count == 0;

    private void Awake() {
        var children = GetComponentsInChildren<FocusableObject>();

        foreach(var child in children) {
            AddChild(child);
        }
    }

    public IFocusable SetFocus()
    {
        if(!IsLeaf) return children.First().SetFocus();
        
        _isFocused = true;
        return this;
    }

    public IFocusable Next() {
        
        return null;
    }

    public void AddChild(IFocusable component) {
        children.Add(component);
    }

    public bool IsFocused() {
        return IsLeaf ? _isFocused : children.Any(child => child.IsFocused());
    }
}
