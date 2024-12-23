using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class CarView : PhysicsObject
{
    // TODO : Hide this
    public Car Car { get; private set; }
    public CarAnimation currentAnimation = null;
    
    public void Initialize(Car car) {
        Car = car;
        ApplyVisual();
    }

    public void Play() {
        if(!Car.CanMove()) return;
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        CarVariables from, to;
        from = to = new(Car.Variables);
        from.isStart = true;

        // starting animation
        yield return Animate(from, to);
        from = new(to);

        while(Car.CanMove()) {
            Car.Move();
            to = Car.Variables;

            yield return Animate(from, to);
            from = to;
        }

        ApplyVisual();
        Car.Reset();
    }

    public IEnumerator Animate(CarVariables from, CarVariables to) {
        currentAnimation = CarAnimationGenerator.Generate(this, from, to);
        currentAnimation.Play();

        yield return YieldDictionary.WaitForSeconds(currentAnimation.duration);
    }

    // TODO : Remove this
    public void ApplyVisual() {
        // direction, position...

        transform.SetLocalPositionAndRotation(
            Car.Variables.position,
            Car.Variables.direction.Rotation());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        //currentAnimation?.Stop();
    }
}

public partial class CarView : IAssignable<Trigger> {

    public void Assign(Trigger trigger)
    {
        trigger.Execute(Car);
    }

    public void Unassign() {
        // TODO : Implement this

    }

    public bool IsAssignable(Trigger trigger)
    {
        // Can't it be done more neatly?
        return trigger.Type switch
        {
            TriggerType.Stop => !Car.Variables.isStop,
            TriggerType.BackUp => !Car.Variables.isBackUp,
            _ => true,
        };
    }
}