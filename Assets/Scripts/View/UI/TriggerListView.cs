#pragma warning disable IDE1006

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerListView : Selectable
{
    public TriggerType selectedTrigger { get; private set; } = TriggerType.None;

    public bool IsSelected => selectedTrigger != TriggerType.None;

    [SerializeField] 
    private Dictionary<TriggerType, TriggerView> _views;

    public void Initialize(LevelGenerator generator) {
        var _triggers = generator.Triggers;

        _views.Values.Select(view => view.enabled = false);
        foreach(var type in _triggers.Types) {
            _views[type].enabled = true;

            _views[type].Count = _triggers[type];
        }

        _triggers.OnTriggerUsed += (trigger) => _views[trigger.Type].Count = _triggers[trigger.Type];
        _triggers.OnTriggerCancelled += (trigger) => _views[trigger.Type].Count = _triggers[trigger.Type];
    }

    public void SelectTrigger(TriggerType type) {
        selectedTrigger = type;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if(_views.Count() == 0) return;

        base.OnSelect(eventData);
        _views.Values.FirstOrDefault().Select();
    }

    protected override void OnEnable() {
        base.OnEnable();
        GameObject.FindGameObjectWithTag("LevelManager")
            .GetComponent<LevelGenerator>()
            .OnLevelGenerated += Initialize;
    }

    protected override void OnDisable() {
        base.OnDisable();
        try {
            GameObject.FindGameObjectWithTag("LevelManager")
                .GetComponent<LevelGenerator>()
                .OnLevelGenerated -= Initialize;
        } catch {
            // ignored
        }
    }
}
