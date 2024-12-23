using System.Collections.Generic;
using UnityEngine;

// save the current state of cars
public class SaveCommand : ICommand
{
    private readonly Dictionary<CarView, CarVariables> _variables;

    public SaveCommand(IEnumerable<CarView> views) {
        _variables = new Dictionary<CarView, CarVariables>();
        foreach(var view in views) {
            _variables.Add(view, new(view.Car.Variables));
        }
    }

    public void Execute()
    {
        // do nothing
    }

    public void Undo()
    {
        foreach(var pair in _variables) {
            pair.Key.Car.SetVariables(pair.Value);
            pair.Key.ApplyVisual();
        }
    }
}