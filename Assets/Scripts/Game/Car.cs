using System;
using UnityEngine;

[Serializable]
public struct CarProperty {
    public Vector2 position;

    [Range(0, 4)]
    public int direction;
    public float speed;
    public bool isStop;
    public bool isBackUp; 
}

public partial class Car : PhysicsObject
{
    private CarProperty _property;
    
    public CarProperty Property { 
        get => _property; 
        private set {
            _property = value;

            ApplyVisualProperty();
        }
    }

    private void ApplyVisualProperty() {
        // direction, position...
        
    }
}

public partial class Car : IAssignable<Trigger>
{
    public void Assign(Trigger trigger)
    {
        
    }

    public bool IsAssignable(Trigger trigger)
    {
        // Can't it be done more neatly?
        return trigger.type switch
        {
            Trigger.Type.Stop => !Property.isStop,
            Trigger.Type.BackUp => !Property.isBackUp,
            _ => true,
        };
    }
}