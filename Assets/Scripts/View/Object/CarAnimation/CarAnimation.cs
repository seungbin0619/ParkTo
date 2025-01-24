using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class CarAnimation {
    protected const float TimeScale = 2f;
    protected CarVariables from, to;
    protected CarView view;
    public readonly float duration;

    private Tween tween;

    public CarAnimation(CarView view, CarVariables from, CarVariables to) {
        this.view = view;
        this.from = new(from);
        this.to = new(to);

        duration = 2 / (from.speed + to.speed) / TimeScale;
    }

    public void Play() {
        view.transform.localPosition = InitialPosition;
        
        tween = Animation();
    }

    public void Stop() {
        tween.Kill();
    }

    private Vector3 InitialPosition => (Vector3)(from.position + to.position) * 0.5f;
    protected virtual Sequence Animation() => DOTween.Sequence();
}

public class CarPhysicsAnimation : CarAnimation {
    private Rigidbody RB => view.RB;

    public CarPhysicsAnimation(CarView view, CarVariables from, CarVariables to) : base(view, from, to) { }

    protected override Sequence Animation() {
        return base.Animation().Append(VelocityAnimation());
    }

    protected virtual TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>  VelocityAnimation() {
        // view.transform.parent.parent.TransformDirection(x) ... too complex
        // TODO : refactor this

        return DOTween.To(() => RB.linearVelocity,
            x => RB.linearVelocity = view.transform.parent.parent.TransformDirection(x) * TimeScale,
            to.speed * (Vector3)to.GetDirection().ToPoint(),
            duration)
            .From(from.speed * (Vector3)from.GetDirection().ToPoint())
            .SetEase(Ease.Linear);
    }
}