using UnityEngine;

public class TestLevelGenerate : MonoBehaviour
{
    public LevelState levelState;
    public LevelView levelView;
    public LevelPack pack;
    void Start() {
        levelState.Initialize(pack, 0);
    }

    public void Go() {
        levelState.Play();
    }
    
    public void Rotate() {
        foreach(var view in levelView.CarViews) {
            view.Car.Variables.Rotate(1);
            view.ApplyVisual();
        }
    }

    void Update() {
        foreach(var view in levelView.CarViews) {
            Debug.DrawLine(view.transform.position, view.transform.position + view.transform.forward * 2f, Color.red);
        }
    }
}
