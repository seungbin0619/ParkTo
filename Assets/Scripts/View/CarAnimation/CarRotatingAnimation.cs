using DG.Tweening;
using UnityEngine;

public class CarRotatingAnimation : CarAnimation {
    public CarRotatingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }

    protected override Tween Animation() {
        Vector3 to = (Vector3)(from.position + this.to.position) * 0.5f;

        Ease easeX, easeZ;
        easeX = from.direction.IsHorizontal() ? Ease.OutSine : Ease.InSine;
        easeZ = easeX == Ease.InSine ? Ease.OutSine : Ease.InSine;
        
        Sequence tween = DOTween.Sequence();
        
        tween.Append(view.transform.DOLocalMoveX(to.x, duration).SetEase(easeX));
        tween.Join(view.transform.DOLocalMoveZ(to.z, duration).SetEase(easeZ));
        tween.Join(view.transform.DOLocalRotateQuaternion(this.to.direction.Rotation(), duration).SetEase(Ease.Linear));

        return tween;
    }
}

