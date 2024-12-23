using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class CarStartingAnimation : CarPhysicsAnimation {
    public CarStartingAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { 
        this.from.speed = 0;
    }
}
