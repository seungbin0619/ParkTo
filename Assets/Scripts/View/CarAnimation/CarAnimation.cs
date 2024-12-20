using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface ICarAnimation {
    public void Play();
    public void Stop();
}

public class CarAnimation : ICarAnimation {
    protected CarVariables from, to;
    protected readonly Transform transform;
    private readonly List<Tween> animations;
    protected Ease positionEase = Ease.Linear, rotationEase = Ease.Linear;
    public readonly float duration;

    public CarAnimation(CarView view, CarVariables from, CarVariables to) {
        transform = view.transform;
        animations = new();

        this.from = from;
        this.to = to;
   
        //t = 2s/(v + v0)
        duration = 2 / (from.speed + to.speed);
    }

    public void Play() {
        animations.Add(PositionAnimation());
        animations.Add(RotationAnimation());
    }

    public void Stop() {
        foreach(var animation in animations) {
            animation.Kill();
        }
    }

    protected virtual Tween PositionAnimation() => 
        transform.DOLocalMove((Vector3)(from.position + to.position) * 0.5f, duration).SetEase(positionEase);

    protected virtual Tween RotationAnimation() => 
        transform.DORotateQuaternion(to.direction.Rotation(), duration).SetEase(rotationEase);
}