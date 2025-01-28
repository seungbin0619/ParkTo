using UnityEngine;

public class LevelInputModule : MonoBehaviour {
    [SerializeField]
    private GameObject _levelManager;
    private LevelAction _action;

    private TriggerListView _triggerListView;
    private ViewInputModule _viewInput;

    void Awake() {
        _action = _levelManager.GetComponent<LevelAction>();

        _viewInput = GetComponentInChildren<ViewInputModule>();
        _triggerListView = GetComponentInChildren<TriggerListView>();
    }

    public void Play() {
        _action.Play();
    }

    public /*async*/ void AssignTrigger() {
        IAssignableView view;
        Trigger trigger;

        // await _viewInput.GetSelectedView...
        // await _triggerListView.GetSelectedTrigger...

        view = _viewInput.selectedView;
        trigger = TriggerGenerator.Generate(_triggerListView.selectedTrigger);

        _action.AssignTrigger(view, trigger);
        
    }
}