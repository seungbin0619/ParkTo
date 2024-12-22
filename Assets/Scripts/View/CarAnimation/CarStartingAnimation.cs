using DG.Tweening;
using UnityEngine;

public class CarStartingAnimation : CarAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }

    protected override Tween Animation() {
        view.RB.linearVelocity = Vector3.zero;
        return base.Animation();
    }
}

