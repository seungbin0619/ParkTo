using System.Collections.Generic;
using System.Linq;

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
        foreach(var (view, variables) in _variables) {
            view.Stop();
            
            view.Car.SetVariables(variables);
            view.ApplyVisual();
        }
    }
}