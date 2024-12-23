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
        CarVariables before, current;
        before = current = new(Car.Variables);
        before.isStart = true;
        
        // starting animation
        yield return Animate(before, current);
        before = new(current);

        while(Car.CanMove()) {
            Car.Move();
            current = Car.Variables;

            yield return Animate(before, current);
            before = current;
        }

        ApplyVisual();
        Car.Reset();
    }

    public IEnumerator Animate(CarVariables before, CarVariables current) {
        currentAnimation = CarAnimationGenerator.Generate(this, before, current);
        currentAnimation.Play();

        yield return YieldDictionary.WaitForSeconds(currentAnimation.duration);
    }

    // TODO : Remove this
    public void ApplyVisual() {
        // direction, position...

        transform.localPosition = Car.Variables.position;
        transform.rotation = Car.Variables.direction.Rotation();
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