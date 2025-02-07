using UnityEngine;
using UnityEngine.UI;

public class LevelInputModule : MonoBehaviour {
    private GameObject _levelManager;
    private LevelAction _action;
    private TriggerListView _triggerListView;
    private ViewInputModule _viewInput;
    private bool _isWaiting;

    void Awake() {
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        _action = _levelManager.GetComponent<LevelAction>();

        _viewInput = GetComponentInChildren<ViewInputModule>();
        _triggerListView = GetComponentInChildren<TriggerListView>();
    }

    public void Play() {
        _action.Play();
    }

    public void Undo() {
        _action.Undo();
    }

    public void Restart() {
        // implement
        
    }

    public async void AssignTrigger(IAssignableView view) {
        if(_isWaiting) return;
        _isWaiting = true;

        Trigger trigger = TriggerGenerator.Generate(
            await _triggerListView.GetSelectedAsync());

        AssignTrigger(view, trigger);

        _viewInput.Reject();
        _triggerListView.Reject();

        _isWaiting = false;
    }

    public async void AssignTrigger(TriggerType type) {
        if(_isWaiting) return;
        _isWaiting = true;

        IAssignableView view = await _viewInput.GetSelectedAsync();
        Trigger trigger = TriggerGenerator.Generate(type);

        AssignTrigger(view, trigger);

        _viewInput.Reject();
        _triggerListView.Reject();

        _isWaiting = false;
    }

    public void AssignTrigger(IAssignableView view, Trigger trigger) {
        Debug.Log(view + " " + trigger.Type);

        _action.AssignTrigger(view, trigger);
    }
}