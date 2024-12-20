using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface ICarAnimation {
    public void Play();
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
        if(from.position != to.position) { 
            animations.Add(PositionAnimation.SetEase(positionEase));
        }

        if(from.direction != to.direction) {
            animations.Add(RotationAnimation.SetEase(rotationEase));
        }
    }

    protected virtual Tweener PositionAnimation => transform.DOLocalMove((from.position + to.position) * 0.5f, duration);
    protected virtual Tweener RotationAnimation => transform.DORotateQuaternion(to.direction.Rotation(), duration);
}

public class CarMovingAnimation : CarAnimation {
    public CarMovingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}

public class CarStartingAnimation : CarAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.InCubic;
    }
}

public class CarStoppingAnimation : CarAnimation {
    public CarStoppingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.OutCubic;
        rotationEase = Ease.OutCubic;
    }

    protected override Tweener PositionAnimation => transform.DOLocalMove(to.position, duration).SetEase(positionEase);
}

public class CarRotatingAnimation : CarAnimation {
    public CarRotatingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}

public class CarAcceleratingAnimation : CarAnimation {
    public CarAcceleratingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
}