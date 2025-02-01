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

    public async void AssignTrigger(IAssignableView view) {
        // IAssignableView view = await _viewInput.GetSelectedViewAsync();
        Trigger trigger = TriggerGenerator.Generate(await _triggerListView.GetSelectedTriggerAsync());
        Debug.Log(trigger);
        
        Debug.Log(view + " " + trigger.Type);
        //_action.AssignTrigger(view, trigger);
    }

    public async void AssignTrigger(TriggerType type) {
        IAssignableView view = await _viewInput.GetSelectedViewAsync();
        Trigger trigger = TriggerGenerator.Generate(type);

        Debug.Log(view + " " + trigger.Type);
        //_action.AssignTrigger(view, trigger);
    }
}