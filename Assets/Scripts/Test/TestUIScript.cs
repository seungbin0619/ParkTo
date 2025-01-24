using UnityEngine;

public class TestUIScript : MonoBehaviour
{
    private LevelState _levelState;
    private LevelView _levelView;
    private LevelGenerator _levelGenerator;

    void Awake() {
        var levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");

        _levelGenerator = levelManagerObject.GetComponent<LevelGenerator>();
        _levelState = levelManagerObject.GetComponent<LevelState>();
        _levelView = levelManagerObject.GetComponent<LevelView>();
    }

    public void Message(string message)
    {
        Debug.Log(message);
    }

    public void Play() {
        _levelState.Play();
    }
}
