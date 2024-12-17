using System.Collections;
using DG.Tweening;
using UnityEngine;

public partial class CarView : PhysicsObject
{
    // TODO : Hide this
    public Car Car { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
    }

    public void Initialize(Car car) {
        Car = car;

        ApplyVisual();
    }

    public void Play() {
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        // Vector3 from = Car.Variables.position, to;

        // while(!Car.Variables.isStop) {
        //     Car.Move();
        //     to = Car.Variables.position; from.y = to.y = transform.localPosition.y;
        //     Debug.Log(to);

        //     //transform.localPosition = from.XZY();
        //     DOTween.To(() => transform.localPosition, x => transform.localPosition = x, to, 1 / Car.Variables.speed);

        //     yield return YieldDictionary.WaitForSeconds(1 / Car.Variables.speed);
            
        //     //ApplyVisual();
        //     from = to;
        // }

        yield return null;
    }

    public void ApplyVisual() {
        // direction, position...
        transform.localPosition = Car.Variables.position + Vector3.up;

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