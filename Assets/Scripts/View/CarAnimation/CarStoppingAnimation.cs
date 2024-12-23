using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class CarStoppingAnimation : CarPhysicsAnimation {
    public CarStoppingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) {
        this.to.speed = 0;
    }

    protected override Sequence Animation()
    {
        return base.Animation().Join(
            view.transform.DOLocalRotateQuaternion(
                Quaternion.LookRotation(to.direction.ToPoint()), 
                duration).SetEase(Ease.OutQuad)
        );
    }
}