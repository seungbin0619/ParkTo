using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SelectableList<T> : Selectable, ISubmitHandler {
    protected abstract string TargetScene { get; }

    protected T _selected, _submitted;

    protected bool HasValue(T a) {
        return !EqualityComparer<T>.Default.Equals(a, default);
    }

    public async Task<T> GetSelectedAsync() {
        if(HasValue(_submitted)) return _submitted;

        ScenePriorityManager.current.SetHighestPriority(TargetScene);

        await Task.Run(() => {
            while(!HasValue(_submitted));
        });

        ScenePriorityManager.current.ResetPriority(TargetScene);
        return _submitted;
    }

    public virtual void Select(T target) {
        _selected = target;
    }

    public void Reject() {
        _submitted = default;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnSubmitted();
    }

    public virtual void OnSubmitted() {
        if(!HasValue(_selected)) return;
    
        _submitted = _selected;
    }
}