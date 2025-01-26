using System.Collections.Generic;
using UnityEngine;

public partial class LevelAction : MonoBehaviour {
    private LevelState _state;
    private LevelView _view;
    private Stack<ICommand> _commands;

    public void Initialize() {
        _state = GetComponent<LevelState>();
        _view = GetComponent<LevelView>();
        
        _commands = new Stack<ICommand>();
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
        return !_state.IsPlaying;
    }
}

public partial class LevelAction {
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