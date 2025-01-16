using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

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
                UpdateCarState(); // isStop 상태 업데이트

                foreach(var view in _views) {
                    if(!view.IsWaitingAnimate) continue;
                    view.IsWaitingAnimate = false;
                }
            }

            yield return YieldDictionary.WaitForEndOfFrame;
        }

        _coroutine = null;
    }

    private void UpdateCarState() {
        const int maxUpdateLoopCount = 99;

        bool updated = true;
        int loopCount = 0;
        
        while(updated && loopCount++ < maxUpdateLoopCount) {
            updated = false;

            foreach(var view in _views) {
                if(!view.Car.CanMove()) continue; // 이미 멈췄으면 끝.
                
                // MEMO: 어떻게 줄일 수 없을까?
                var ground = view.Car.Ground.Next(view.Car.Variables.GetDirection());
                if(ground.IsEnterable) continue;

                view.Car.Stop();
                updated = true;
            }
        }

        if(loopCount >= maxUpdateLoopCount) {
            throw new Exception($"Warning: Detected excessive update iterations. The loop exceeded the configured limit of maxUpdateLoopCount({maxUpdateLoopCount}). Please check your update logic.");
        }
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