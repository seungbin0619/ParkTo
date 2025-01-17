using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class CarView : PhysicsObject
{
    // TODO : Hide this
    public Car Car { get; private set; }
    public Point position => Car.Variables.position;

    public bool IsWaitingAnimate = false;
    private WaitWhile _waitAnimate;

    public bool IsAnimating => _coroutine != null;
    private CarAnimation _animation = null;
    private Coroutine _coroutine = null;

    void OnDrawGizmos() {
        if(!Car.CanMove()) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.4f, 0.2f);
    }
    
    public void Initialize(Car car) {
        Car = car;
        ApplyVisual();
    }

    public void Play() {
        if(!Car.CanMove()) return;

        _coroutine = StartCoroutine(Move());
    }

    private IEnumerator Move() {
        CarVariables from, to;
        
        from = to = new(Car.Variables);
        from.isStart = true;

        // starting animation
        yield return Animate(from, to);
        from = new(to);

        while(!Car.IsStopped) {
            Car.Move();
            
            IsWaitingAnimate = true;
            yield return _waitAnimate ??= new WaitWhile(() => IsWaitingAnimate);

            to = Car.Variables;
            yield return Animate(from, to);
            from = to;
        }

        ApplyVisual();
        Stop();
    }

    public void Stop() {
        if(_coroutine != null) {
            _animation?.Stop();
            StopCoroutine(_coroutine);

            IsWaitingAnimate = false;
            _animation = null;
            _coroutine = null;

            Car.Reset();
        }

        RB.linearVelocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
    }

    public IEnumerator Animate(CarVariables from, CarVariables to) {
        _animation = CarAnimationGenerator.Generate(this, from, to);
        _animation.Play();

        yield return YieldDictionary.WaitForSeconds(_animation.duration);
    }

    public void ApplyVisual() {
        transform.SetLocalPositionAndRotation(
            Car.Variables.position,
            Car.Variables.direction.Rotation());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        // Stop();
    }
}

public partial class CarView : IAssignableView {
    public void SetFocus() {
        Debug.Log("CarView selected!");
    }

    public void LostFocus() {
        Debug.Log("CarView unselected!");
    }

    public void Assign(Trigger trigger)
    {
        trigger.Execute(Car);
        ApplyVisual();
    }

    public void Unassign(Trigger trigger) {
        trigger.Abort(Car);
        ApplyVisual();
    }

    public bool IsAssignable(Trigger trigger)
    {
        // Can't it be done more neatly? ... no
        return trigger.Type switch
        {
            TriggerType.Stop => !Car.Variables.isStop,
            TriggerType.BackUp => !Car.Variables.isBackUp,
            TriggerType.Accelerate | TriggerType.Decelerate => Car.Variables.speed == 1f,
            _ => true,
        };
    }
}