#pragma warning disable IDE1006

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerListView : SelectableList<TriggerType>
{
    protected override string TargetScene => "LevelTriggerUI";

    private LevelInputModule _module;

    [SerializeField] 
    private SerializedDictionary<TriggerType, TriggerView> _views;
    private Triggers _triggers;

    protected override void Awake()
    {
        base.Awake();

        if(!Application.isPlaying) return;
        _module = GetComponentInParent<LevelInputModule>();
    }

    public void Initialize(LevelGenerator generator) {
        if(!generator.HasInitialized) return;
        // Debug.Log("initialized");

        _triggers = generator.Triggers;

        _views.Values.Select(view => _triggers.Types.Contains(view.Type) ? view : null).ToList().ForEach((view) => {
            if(view == null) return;
            view.gameObject.SetActive(false);
        });

        foreach(var type in _triggers.Types) {
            _views[type].gameObject.SetActive(true);

            _views[type].Count = _triggers[type];
        }

        _triggers.OnTriggerUsed += (type) => _views[type].Count = _triggers[type];
        _triggers.OnTriggerCancelled += (type) => _views[type].Count = _triggers[type];
    }
    public override void OnSubmitted()
    {
        if(!_triggers.IsEnabled(_selected)) return;
        base.OnSubmitted();

        _module.AssignTrigger();  
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        // refactor..
        IEnumerator SelectFirstTrigger() {
            yield return new WaitWhile(() => EventSystem.current.alreadySelecting);

            _views.Values.FirstOrDefault(view => view.interactable).Select();
            interactable = false;
        }

        StartCoroutine(SelectFirstTrigger());
    }

    protected override void OnEnable() {
        base.OnEnable();
        if(!Application.isPlaying) return;
        
        var generator = GameObject.FindGameObjectWithTag("LevelManager")
            .GetComponent<LevelGenerator>();

        Initialize(generator);
        generator.OnLevelGenerated += Initialize;
    }

    protected override void OnDisable() {
        base.OnDisable();

        try {
            GameObject.FindGameObjectWithTag("LevelManager")
                .GetComponent<LevelGenerator>()
                .OnLevelGenerated -= Initialize;
        } catch { /* ignored.. */ }
    }
}
