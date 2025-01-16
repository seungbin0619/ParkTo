using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// save the current state of cars
public class PlayCommand : ICommand
{
    private readonly LevelState _state;
    private readonly Dictionary<CarView, CarVariables> _variables;
    private readonly IEnumerable<CarView> _views;
    private Coroutine _coroutine;

    public PlayCommand(LevelState _state, IEnumerable<CarView> _views) {
        _variables = new Dictionary<CarView, CarVariables>();

        this._state = _state;
        this._views = _views;
    }

    public bool Condition() {
        return _views.Any(view => view.Car.CanMove()) && !_views.Any(view => view.IsAnimating);
    }

    public void Execute() {
        foreach(var view in _views) {
            _variables.Add(view, new(view.Car.Variables));
        }

        foreach(var view in _views) {
            view.Play();
        }

        _coroutine = _state.StartCoroutine(Move());
    }

    private IEnumerator Move() {
        while(_views.Any(view => view.IsAnimating)) {
            var waitViews = _views.Where(view => view.IsWaitingAnimate);

            if(waitViews.Count() > 0) {
                foreach(var view in _views) {
                    if(!view.IsWaitingAnimate) continue;
                    view.IsWaitingAnimate = false;
                }
            }

            yield return YieldDictionary.WaitForEndOfFrame;
        }

        _coroutine = null;
    }

    public void Undo() {
        if(_coroutine != null) {
            _state.StopCoroutine(_coroutine);
        }

        foreach(var (view, variables) in _variables) {
            view.Stop();
            
            view.Car.SetVariables(variables);
            view.ApplyVisual();
        }
    }
}