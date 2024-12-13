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
        levelState.Move();
    }
    
    public void Rotate() {
        foreach(var view in levelView.CarViews) {
            view.Car.Variables.Rotate(1);
            view.ApplyVisual();
        }
    }
}
