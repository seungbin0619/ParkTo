using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PriorityObserver : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    [SerializeField]
    private List<MonoBehaviour> _targets;
    
    public bool State { get; private set; }

    protected virtual void Awake() {
        if(!Application.isPlaying) return;

        if(_sceneName == "") _sceneName = gameObject.scene.name;
    }
    
    protected virtual void OnEnable() {
        if(!Application.isPlaying) return;

        ScenePriorityManager.current.OnScenePriorityChanged += SetState;
    }

    protected virtual void OnDisable() {
        if(!Application.isPlaying) return;
        
        try {
            ScenePriorityManager.current.OnScenePriorityChanged -= SetState;
        } catch { /* ignored */ }
    }

    protected virtual void OnStateUpdate(bool state) {
        foreach(var target in _targets) {
            target.enabled = state;
        }
    }

    protected void SetState() {
        State = ScenePriorityManager.current.IsHighestPriority(_sceneName);

        OnStateUpdate(State);
    }
}