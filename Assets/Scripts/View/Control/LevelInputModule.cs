using UnityEngine;
using UnityEngine.UI;

public class LevelInputModule : MonoBehaviour {
    private GameObject _levelManager;
    private LevelAction _action;

    private TriggerListView _triggerListView;
    private ViewInputModule _viewInput;

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

    public async void AssignTrigger() {
        IAssignableView view = await _viewInput.GetSelectedView();
        Trigger trigger = await _triggerListView.GetSelectedTrigger();

        _action.AssignTrigger(view, trigger);
    }
}