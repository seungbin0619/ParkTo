using DG.Tweening;
using UnityEngine;

public class TestLevelGenerate : MonoBehaviour
{
    public LevelState levelState;
    public LevelView levelView;
    public LevelPack pack;
    void Start() {
        levelState.Initialize(pack, 0);
    }

    public void Play() {
        levelState.Play();
    }
    
    public void Undo() {
        levelState.Undo();
    }

    void Update() {
        foreach(var view in levelView.CarViews) {
            Debug.DrawLine(view.transform.position, view.transform.position + view.transform.forward * 2f, Color.red);
        }
    }
}
