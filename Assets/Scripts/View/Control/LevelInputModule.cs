using UnityEngine;

public class LevelInputModule : MonoBehaviour {
    [SerializeField]
    private GameObject _levelManager;
    private LevelState _levelState;
    private LevelView _levelView;
    private LevelGenerator _levelGenerator;

    void Awake() {
        _levelGenerator = _levelManager.GetComponent<LevelGenerator>();
        _levelState = _levelManager.GetComponent<LevelState>();
        _levelView = _levelManager.GetComponent<LevelView>();
    }

    
}