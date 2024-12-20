using DG.Tweening;

public class CarStartingAnimation : CarAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        positionEase = Ease.InQuad;
    }
}

