using UnityEngine;

public class TestUIScript : MonoBehaviour
{
    private LevelState _levelState;
    private LevelAction _levelAction;
    private LevelView _levelView;
    private LevelGenerator _levelGenerator;

    void Awake() {
        var levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");

        _levelGenerator = levelManagerObject.GetComponent<LevelGenerator>();
        _levelState = levelManagerObject.GetComponent<LevelState>();
        _levelView = levelManagerObject.GetComponent<LevelView>();
        _levelAction = levelManagerObject.GetComponent<LevelAction>();
    }

    public void Message(string message)
    {
        Debug.Log(message);
    }

    public void Play() {
        _levelAction.Play();
    }
}
