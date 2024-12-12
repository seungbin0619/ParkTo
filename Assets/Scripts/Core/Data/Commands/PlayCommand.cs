using System.Collections.Generic;
using UnityEngine;

public class PlayCommand : ICommand
{
    public Dictionary<CarView, CarVariables> variables;

    public PlayCommand() {
        
    }

    public void Execute()
    {
        
    }

    public void Undo()
    {
        foreach(var pair in variables) {
            pair.Key.Car.SetVariables(pair.Value);
            pair.Key.ApplyVisual();
        }
    }
}