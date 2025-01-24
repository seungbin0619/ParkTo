using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 레벨 상태 정보들을 총체적으로 담는 클래스.
/// ex) 클리어 여부, 차, 트리거, ...
/// </summary>
public partial class LevelState : MonoBehaviour {
    private LevelGenerator _generator;
    private LevelView _view;

    public bool IsPlaying => !_view.CarViews.Any(view => view.IsAnimating);
    
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
        if(!IsPlayable()) return;

        Execute(new PlayCommand(this, _view.CarViews));
    }

    private bool IsPlayable() {
        return true;
    }

    public void AssignTrigger(IAssignable<Trigger> target, Trigger trigger) {
        if(!IsAssignable()) return;
        Execute(new AssignTriggerCommand(target, trigger));
    }

    private bool IsAssignable() {
        return !IsPlaying;
    }
}

// about commands
public partial class LevelState {
    private Stack<ICommand> _commands;

    private void Execute(ICommand command) {
        if(!CanExcecute()) return;
        if(!command.Condition()) return;

        command.Execute();
        _commands.Push(command);
    }

    // TODO: implement this
    private bool CanExcecute() {
        return true;
    }

    public void Undo() {
        if(_commands.Count == 0) {
            return;
        }

        var command = _commands.Pop();
        command.Undo();
    }
}