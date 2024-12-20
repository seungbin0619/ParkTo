using UnityEngine;

public static class CarAnimationGenerator {
    private enum AnimationType {
        None = -1,
        Starting,
        Stoping,
        Rotating,
        Accelerating
    }
    
    public static CarAnimation Generate(CarView view, CarVariables from, CarVariables to) {
        AnimationType type = GetType(from, to);

        return type switch
        {
            AnimationType.Starting => new CarStartingAnimation(view, from, to),
            AnimationType.Stoping => new CarStoppingAnimation(view, from, to),
            AnimationType.Rotating => new CarRotatingAnimation(view, from, to),
            AnimationType.Accelerating => new CarMovingAnimation(view, from, to),
            _ => new CarAnimation(view, from, to)
        };
    }

    private static AnimationType GetType(CarVariables from, CarVariables to) {
        if(from.isStart) return AnimationType.Starting;
        if(to.isStop) return AnimationType.Stoping;
        
        if(from.direction != to.direction) return AnimationType.Rotating;
        if(from.speed != to.speed) return AnimationType.Accelerating;

        return AnimationType.None;
    }
}