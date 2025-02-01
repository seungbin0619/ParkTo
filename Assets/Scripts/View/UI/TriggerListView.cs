#pragma warning disable IDE1006

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerListView : Selectable, ISubmitHandler
{
    private LevelInputModule _module;
    public TriggerType selectedTrigger { get; private set; } = TriggerType.None;

    [SerializeField] 
    private SerializedDictionary<TriggerType, TriggerView> _views;

    protected override void Awake()
    {
        base.Awake();

        _module = GetComponentInParent<LevelInputModule>();
    }

    public void Initialize(LevelGenerator generator) {
        if(!generator.HasInitialized) return;
        // Debug.Log("initialized");
        var _triggers = generator.Triggers;

        _views.Values.Select(view => _triggers.Types.Contains(view.Type) ? view : null).ToList().ForEach((view) => {
            if(view == null) return;
            view.gameObject.SetActive(false);
        });

        foreach(var type in _triggers.Types) {
            _views[type].gameObject.SetActive(true);

            _views[type].Count = _triggers[type];
        }

        _triggers.OnTriggerUsed += (trigger) => _views[trigger.Type].Count = _triggers[trigger.Type];
        _triggers.OnTriggerCancelled += (trigger) => _views[trigger.Type].Count = _triggers[trigger.Type];
    }

    public async Task<TriggerType> GetSelectedTriggerAsync() {
        if(selectedTrigger != TriggerType.None) {
            return selectedTrigger;
        }

        // open trigger select ui...
        ScenePriorityManager.current.SetHighestPriority("LevelTriggerUI");

        await Task.Run(() => {
            Debug.Log("Wait for select trigger");
            while(selectedTrigger == TriggerType.None);
            // Debug.Log(selectedTrigger);
            // ...
            Debug.Log("Trigger Selected");
        });

        ScenePriorityManager.current.ResetAllPriorities();
        return selectedTrigger;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AssignTrigger();
    }

    public void AssignTrigger() {
        if(selectedTrigger == TriggerType.None) return;

        _module.AssignTrigger(selectedTrigger);
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

    public void SelectTrigger(TriggerType type) {
        selectedTrigger = type;

        interactable = selectedTrigger == TriggerType.None;
    }

    protected override void OnEnable() {
        base.OnEnable();

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
        } catch {
            // ignored
        }
    }
}
