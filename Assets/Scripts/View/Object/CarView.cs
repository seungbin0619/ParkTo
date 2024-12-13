using UnityEngine;

public partial class CarView : PhysicsObject
{
    public Car Car { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
    }

    public void Initialize(Car car) {
        Car = car;

        ApplyVisual();
    }

    public void ApplyVisual() {
        // direction, position...
        transform.localPosition = Car.Variables.position;
        transform.rotation = Car.Variables.direction.Rotation();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        
    }
}

public partial class CarView : IAssignable<Trigger> {
    public void Assign(Trigger trigger)
    {
        trigger.Execute(Car);
    }

    public bool IsAssignable(Trigger trigger)
    {
        // Can't it be done more neatly?
        return trigger.type switch
        {
            Trigger.Type.Stop => !Car.Variables.isStop,
            Trigger.Type.BackUp => !Car.Variables.isBackUp,
            _ => true,
        };
    }
}