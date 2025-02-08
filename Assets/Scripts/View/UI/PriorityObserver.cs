using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PriorityObserver : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    protected virtual void Awake() {
        if(_sceneName == "") _sceneName = gameObject.scene.name;
    }
    
    protected virtual void OnEnable() {
        ScenePriorityManager.current.OnScenePriorityChanged += SetState;
    }

    protected virtual void OnDisable() {
        try {
            ScenePriorityManager.current.OnScenePriorityChanged -= SetState;
        } catch { /* ignored */ }
    }

    protected virtual void UpdateState(bool state) {
        throw new NotImplementedException();
    }

    protected void SetState() {
        UpdateState(ScenePriorityManager.current.IsHighestPriority(_sceneName));
    }
}