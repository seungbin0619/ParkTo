using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 레벨 상태 정보들을 총체적으로 담는 클래스.
/// ex) 클리어 여부, 차, 트리거, ...
/// </summary>
public partial class LevelState : MonoBehaviour {
    private Stack<ICommand> _commands;
    private LevelGenerator _generator;
    private LevelView _view;

    void Awake() {
        _generator = GetComponent<LevelGenerator>();
        _view = GetComponent<LevelView>();
    }

    public void Initialize(LevelPack levelPack, int index) {
        if(index < 0 || index >= levelPack.levels.Count) {
            return;
        }

        _commands = new Stack<ICommand>();

        _generator.Initialize(levelPack, index);
        _view.Initialize(levelPack.style);
        _view.CreateView();
    }

    public void Play() {
        _commands.Push(new SaveCommand(_view.CarViews));
        foreach(var view in _view.CarViews) {
            view.Play();
            //view.ApplyVisual();
        }
    }

    public void AssignTrigger(IAssignable<Trigger> target, Trigger trigger) {
        _commands.Push(new AssignTriggerCommand(target, trigger));
        
        trigger.Assign(target);
    }

    public void Undo() {
        if(_commands.Count == 0) {
            return;
        }

        var command = _commands.Pop();
        command.Undo();
    }
}