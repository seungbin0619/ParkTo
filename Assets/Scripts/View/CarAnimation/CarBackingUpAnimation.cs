using DG.Tweening;
using UnityEngine;

public class CarBackingUpAnimation : CarAnimation {
    public CarBackingUpAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 

    }

    protected override Tween Animation() {
        Vector3 to = view.transform.localPosition;
        
        Sequence tween = DOTween.Sequence();
        tween.Append(view.transform.DOLocalMove((from.position + to) * 0.5f, duration * 0.5f).SetEase(Ease.OutQuad));
        tween.Append(view.transform.DOLocalMove(to, duration * 0.5f).SetEase(Ease.InQuad));

        return tween;
    }
}

