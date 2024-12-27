using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// save the current state of cars
public class PlayCommand : ICommand
{
    private readonly Dictionary<CarView, CarVariables> _variables;
    private readonly IEnumerable<CarView> views;

    public PlayCommand(IEnumerable<CarView> views) {
        _variables = new Dictionary<CarView, CarVariables>();
        this.views = views;
    }

    public bool Condition()
    {
        return views.Any(view => view.Car.CanMove());
    }

    public void Execute()
    {
        foreach(var view in views) {
            _variables.Add(view, new(view.Car.Variables));
        }

        foreach(var view in views) {
            view.Play();
        }
    }

    public void Undo()
    {
        foreach(var pair in _variables) {
            pair.Key.Car.SetVariables(pair.Value);
            pair.Key.ApplyVisual();
        }
    }
}