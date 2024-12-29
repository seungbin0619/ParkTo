using System.Linq;
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

    public void AssignTrigger() {
        var view = levelView.CarViews.First();
        var trigger = TriggerGenerator.Generate(TriggerType.TurnRight);

        levelState.AssignTrigger(view, trigger);
    }

    void Update() {
        foreach(var view in levelView.CarViews) {
            Debug.DrawLine(view.transform.position, view.transform.position + view.transform.forward * 2f, Color.red);
        }
    }
}
