using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TestLevelGenerate : MonoBehaviour
{
    public LevelState levelState;
    public LevelAction levelAction;
    public LevelView levelView;
    public LevelPack pack;

    void Start() {
        Reload();
    }

    public void Reload() {
        levelState.Initialize(pack, 0);
    }

    public void Play() {
        levelAction.Play();
    }
    
    public void Undo() {
        levelAction.Undo();
    }

    public void AssignTrigger() {
        var view = levelView.CarViews.First();
        var trigger = TriggerGenerator.Generate(TriggerType.TurnRight);

        levelAction.AssignTrigger(view, trigger);
    }

    void Update() {
        foreach(var view in levelView.CarViews) {
            Debug.DrawLine(view.transform.position, view.transform.position + view.transform.forward * 2f, Color.red);
        }
    }
}
