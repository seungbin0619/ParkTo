using DG.Tweening;
using UnityEngine;

public class CarAcceleratingAnimation : CarAnimation {
    public CarAcceleratingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }

    protected override Tween Animation()
    {
        // TODO : fix this
        return view.transform.DOLocalMove((Vector3)(from.position + to.position) * 0.5f, duration).SetEase(Ease.Linear);
    }
}