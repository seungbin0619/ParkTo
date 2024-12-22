using UnityEngine;
using DG.Tweening;

public class CarStoppingAnimation : CarAnimation {
    public CarStoppingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }
    protected override Tween Animation() {
        view.RB.linearVelocity = from.speed * from.direction.ToPoint();
        return DOTween.To(
            () => view.RB.linearVelocity,
            x => view.RB.linearVelocity = x,
            Vector3.zero,
            duration)
            .SetEase(Ease.Linear);
    }
}

