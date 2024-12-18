using UnityEngine;

public interface ICarAnimation {
    public void Play();
}

public abstract class CarMoveAnimation : ICarAnimation {
    protected Point from, to;
    
    public void Play() {
        
    }
}

// public abstract class CarMoveAnimation : ICarAnimation {
//     protected Point from, to;
//     protected Ease ease;

//     public CarMoveAnimation(Point from, Point to) {
//         this.from = from;
//         this.to = to;

//         ease = Ease.Linear;
//     }

//     public void Play(CarView view, float duration) {
//         Transform transform = view.transform;
//         Vector3 from = this.from, to = this.to;

//         DOTween.To(() => transform.localPosition, x => transform.localPosition = x, (from + to) * 0.5f, duration);
//     }
// }

// public class CarStartingAnimation : CarMoveAnimation {
//     public CarStartingAnimation(Point from, Point to) : base(from, to) {
//         ease = Ease.InCubic;
//     }
// }

// public class CarStopingAnimation : CarMoveAnimation {
//     public CarStopingAnimation(Point from, Point to) : base(from, to) {
//         ease = Ease.OutCubic;
//     }
// }

// public class CarRotatingAnimation : CarMoveAnimation {
//     public CarRotatingAnimation(Point from, Point to) : base(from, to) {

//     }
// }

// public class CarAcceleratingAnimation : CarMoveAnimation {
//     public CarAcceleratingAnimation(Point from, Point to) : base(from, to) {
        
//     }
// }