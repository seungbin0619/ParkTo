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
    public TriggerType selectedTrigger { get; private set; } = TriggerType.None;

    [SerializeField] 
    private SerializedDictionary<TriggerType, TriggerView> _views;

    public void Initialize(LevelGenerator generator) {
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

    public async Task<Trigger> GetSelectedTrigger() {
        if(selectedTrigger != TriggerType.None) {
            return TriggerGenerator.Generate(selectedTrigger);
        }

        // open trigger select ui...


        await Task.Run(() => {
            Debug.Log("Wait for select trigger");
            while(selectedTrigger != TriggerType.None);

            // ...
            Debug.Log("Trigger Selected");
        });

        return TriggerGenerator.Generate(selectedTrigger);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
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
