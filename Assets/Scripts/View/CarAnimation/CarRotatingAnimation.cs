using DG.Tweening;
using UnityEngine;

public class CarRotatingAnimation : CarAnimation {
    public CarRotatingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }

    protected override Tween PositionAnimation() {
        Vector3 to = (Vector3)(from.position + this.to.position) * 0.5f;

        Ease easeX, easeZ;
        easeX = from.direction.IsHorizontal() ? Ease.InSine : Ease.OutSine;
        easeZ = easeX == Ease.InSine ? Ease.OutSine : Ease.InSine;
        
        Sequence tween = DOTween.Sequence();
        tween.Append(transform.DOLocalMoveX(to.x, duration)).SetEase(easeX);
        tween.Join(transform.DOLocalMoveZ(to.z, duration)).SetEase(easeZ);

        return tween;
    }
}

