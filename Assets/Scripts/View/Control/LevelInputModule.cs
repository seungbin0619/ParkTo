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

    public void AssignTrigger() {
        // _action.AssignTrigger(ViewInputSystem.current.selectedView, )
    }
}