using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListView : MonoBehaviour
{
    Triggers _triggers;

    [NonSerialized]
    public TriggerType selectedTrigger = TriggerType.None;

    public bool IsSelected => selectedTrigger != TriggerType.None;

    private List<TriggerView> _views;

    [SerializeField] 
    private TriggerView _triggerViewPrefab;

    public void Initialize(LevelGenerator generator) {
        // Debug.Log("trigger list initialized");
        
        _triggers = generator.Triggers;
        _views = new();

        foreach(var type in _triggers.Types) {
            uint count = _triggers[type];

            var view = Instantiate(_triggerViewPrefab, transform);
            view.Initialize(type, count);

            _views.Add(view);
        }
    }

    void OnEnable() {
        GameObject.FindGameObjectWithTag("LevelManager")
            .GetComponent<LevelGenerator>()
            .OnLevelGenerated += Initialize;
    }

    void OnDisable() {
        try {
            GameObject.FindGameObjectWithTag("LevelManager")
                .GetComponent<LevelGenerator>()
                .OnLevelGenerated -= Initialize;
        } catch {
            // ignored
        }
    }
}
