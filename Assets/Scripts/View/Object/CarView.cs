using System.Collections;
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
        // starting animation
        yield return Animate(Car.Variables);

        while(Car.CanMove()) {
            Car.Move();

            yield return Animate(Car.Variables);
        }

        // ApplyVisual();
        Car.Reset();
    }

    public IEnumerator Animate(CarVariables from) {
        CarVariables to = Car.Variables.Next();

        currentAnimation = CarAnimationGenerator.Generate(this, from, to);
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
        return trigger.type switch
        {
            Trigger.Type.Stop => !Car.Variables.isStop,
            Trigger.Type.BackUp => !Car.Variables.isBackUp,
            _ => true,
        };
    }
}