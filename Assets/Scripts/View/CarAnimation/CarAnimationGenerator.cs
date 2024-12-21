using UnityEngine;

public static class CarAnimationGenerator {
    private enum AnimationType {
        None = -1,
        Starting,
        Stoping,
        Rotating,
        BackingUp,
        Accelerating
    }
    
    public static CarAnimation Generate(CarView view, CarVariables before, CarVariables current) {
        AnimationType type = GetType(before, current);
        Debug.Log(type);

        return type switch
        {
            AnimationType.Starting => new CarStartingAnimation(view, before, current),
            AnimationType.Stoping => new CarStoppingAnimation(view, before, current),
            AnimationType.BackingUp => new CarBackingUpAnimation(view, before, current),
            AnimationType.Rotating => new CarRotatingAnimation(view, before, current),
            AnimationType.Accelerating => new CarAcceleratingAnimation(view, before, current),
            _ => new CarAnimation(view, before, current)
        };
    }

    private static AnimationType GetType(CarVariables from, CarVariables to) {
        if(from.isStart) return AnimationType.Starting;
        if(to.isStop) return AnimationType.Stoping;
        if(!from.isBackUp && to.isBackUp) return AnimationType.BackingUp;
        if(from.direction != to.direction) return AnimationType.Rotating;
        if(from.speed != to.speed) return AnimationType.Accelerating;

        return AnimationType.None;
    }
}