using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LevelInputModule : MonoBehaviour {
    // private GameObject _levelManager;
    private LevelAction _action;
    private TriggerListView _triggerListView;
    private ViewInputModule _viewInput;

    private bool _isWaiting;

    void Awake() {
        var _levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        _action = _levelManager.GetComponent<LevelAction>();

        _viewInput = GetComponentInChildren<ViewInputModule>();
        _triggerListView = GetComponentInChildren<TriggerListView>();
    }

    public void OnPlay() => Play();

    public void Play() {
        _action.Play();
    }

    public void OnRestart() => Restart();

    public void Restart() {
        _action.Restart();
    }

    public void OnUndo() => Undo();

    public void Undo() {
        _action.Undo();
    }

    public void OnCancel() {
        if(!enabled) return;
        if(_isWaiting) return;

        Debug.Log("Cancelled!");
    }

    public void OnRotateClockwise() => Rotate(1);
    public void OnRotateInclockwise() => Rotate(-1);

    public void Rotate(int direction) {
        _action.Rotate(direction);
    }

    public async void AssignTrigger() {
        if(_isWaiting) return;
        _isWaiting = true;
        ScenePriorityManager.current.SetHighestPriority("LevelManager", "LevelTriggerUI");

        /////////////////////////////////////////////////////////////////////////////////

        IAssignableView view = await _viewInput.GetSelectedAsync();
        Trigger trigger = TriggerGenerator.Generate(await _triggerListView.GetSelectedAsync());

        AssignTrigger(view, trigger);

        _viewInput.Reject();
        _triggerListView.Reject();

        /////////////////////////////////////////////////////////////////////////////////

        ScenePriorityManager.current.ResetPriority("LevelManager", "LevelTriggerUI");
        _isWaiting = false;
    }

    public void AssignTrigger(IAssignableView view, Trigger trigger) {
        if(view == default || trigger == default) return;
        Debug.Log(view + " " + trigger.Type);

        _action.AssignTrigger(view, trigger);
    }
}