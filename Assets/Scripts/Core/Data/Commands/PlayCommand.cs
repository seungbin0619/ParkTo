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

        // foreach(var view in _views) {
        //     view.Play();
        // }

        _state.StartCoroutine(Move());
    }

    private IEnumerator Move() {

        yield return null;
    }

    public void Undo() {
        foreach(var (view, variables) in _variables) {
            view.Stop();
            
            view.Car.SetVariables(variables);
            view.ApplyVisual();
        }
    }
}