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
    private readonly List<Tweener> animations;
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
        animations.Add(PositionAnimation.SetEase(positionEase));
        animations.Add(RotationAnimation.SetEase(rotationEase));
    }

    public void Stop() {
        foreach(var animation in animations) {
            animation.Kill();
        }
    }

    protected virtual Tweener PositionAnimation => transform.DOLocalMove((Vector3)(from.position + to.position) * 0.5f, duration);
    protected virtual Tweener RotationAnimation => transform.DORotateQuaternion(to.direction.Rotation(), duration);
}

public class CarMovingAnimation : CarAnimation {
    public CarMovingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}

public class CarStartingAnimation : CarAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.InQuad;
    }
}

public class CarStoppingAnimation : CarAnimation {
    public CarStoppingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.OutQuad;
        rotationEase = Ease.OutQuad;
    }

    protected override Tweener PositionAnimation => transform.DOLocalMove(from.position, duration);
}

public class CarRotatingAnimation : CarAnimation {
    public CarRotatingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}

public class CarAcceleratingAnimation : CarAnimation {
    public CarAcceleratingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}