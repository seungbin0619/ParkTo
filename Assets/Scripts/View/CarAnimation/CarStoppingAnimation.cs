using DG.Tweening;

public class CarStoppingAnimation : CarAnimation {
    public CarStoppingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.OutQuad;
        rotationEase = Ease.OutQuad;
    }

    protected override Tween PositionAnimation() => 
        transform.DOLocalMove(from.position, duration).SetEase(positionEase);
}

