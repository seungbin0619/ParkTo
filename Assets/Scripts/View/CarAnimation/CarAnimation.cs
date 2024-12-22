using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarAnimation {
    protected const float TimeScale = 2f;

    private Tween tween;
    protected CarView view;
    protected CarVariables from, to;
    public readonly float duration;
    
    public CarAnimation(CarView view, CarVariables from, CarVariables to) {
        this.view = view;
        this.from = from;
        this.to = to;

        duration = 2 / (from.speed + to.speed);
    }

    public void Play() {
        tween = Animation();
    }

    public void Stop() {
        tween.Kill();
        tween = null;
    }

    protected virtual Tween Animation() {
        return DOTween.To(
            () => view.RB.linearVelocity,
            x => view.RB.linearVelocity = x,
            from.direction.ToPoint() * from.speed,
            duration)
            //.From(view.transform.forward * from.speed)
            .SetEase(Ease.Linear);
    }
}