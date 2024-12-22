using DG.Tweening;
using UnityEngine;

public class CarStartingAnimation : CarAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.InQuad;
    }

    protected override Tween PositionAnimation() => 
        transform.DOLocalMove((Vector3)(from.position + to.position - from.direction.ToPoint()) * 0.5f, duration).SetEase(positionEase);
}

