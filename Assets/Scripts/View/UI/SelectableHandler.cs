using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Selectable))]
public class SelectableHandler : MonoBehaviour {
    private Selectable _selectable;

    [SerializeField]
    private string _sceneName;

    void Awake() {
        _selectable = GetComponent<Selectable>();
        if(_sceneName == "") _sceneName = gameObject.scene.name;
    }

    void Start() {
        UpdateInteractable();
    }

    void OnEnable() {
        ScenePriorityManager.current.OnScenePriorityChanged += UpdateInteractable;
    }

    void OnDisable() {
        try {
            ScenePriorityManager.current.OnScenePriorityChanged -= UpdateInteractable;
        } catch { /* ignored */ }
    }

    void UpdateInteractable() {
        if(_selectable == null) return;

        //Debug.Log(ScenePriorityManager.current == null);
        _selectable.enabled = ScenePriorityManager.current.IsHighestPriority(_sceneName);
    }
}