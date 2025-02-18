using System.Linq;
using UnityEngine;

public class Destination {
    private readonly int _color;
    private readonly Point _position;
    private readonly Ground _ground;

    public Destination(DestinationSerializer goal, Ground ground) {
        _color = goal.color;
        _ground = ground;
        _position = goal.position;

        if(_ground.Position != _position) {
            Debug.LogWarning("Destination's Ground position does not match the Position value.");
        }
    }

    public Destination(int color, Point position) {
        _color = color;
        _position = position;
    }

    public bool HasReached() {
        return _ground.Cars.All(car => car.Color == _color);
    }
}