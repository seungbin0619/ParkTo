using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Selectable))]
public class SelectableHandler : MonoBehaviour {
    private Selectable _selectable;
    private string _sceneName;

    void Awake() {
        _selectable = GetComponent<Selectable>();
        _sceneName = gameObject.scene.name;
        
        UpdateInteractable();
    }

    void OnEnable() {
        ScenePriorityManager.current.OnScenePriorityChanged += UpdateInteractable;
    }

    void OnDisable() {
        ScenePriorityManager.current.OnScenePriorityChanged -= UpdateInteractable;
    }

    void UpdateInteractable() {
        if(_selectable == null) return;

        //Debug.Log(ScenePriorityManager.current == null);
        _selectable.enabled = ScenePriorityManager.current.IsHighestPriority(_sceneName);
    }
}