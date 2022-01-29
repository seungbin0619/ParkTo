using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private UnityEngine.UI.Button button;

    private bool beforeCondition = false;

    private Vector3 _targetScale = Vector3.one;
    private Vector3 TargetScale
    {
        get { return _targetScale; }
        set {
            if(_targetScale != value)
            {
                progress = 0;
                _targetScale = value;
            }
        }
    }
    private float progress = 0;

    public void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    public void PrevMove()
    {
        button.enabled = false;
        beforeCondition = false;

        TargetScale = Vector3.zero;
    }

    public void AfterChange()
    {
        if (beforeCondition != MapSystem.isPlayable)
        {
            button.enabled = MapSystem.isPlayable;
            beforeCondition = MapSystem.isPlayable;

            //TargetScale = beforeCondition ? Vector3.one : Vector3.zero;
        }
    }

    public void Update()
    {
        button.enabled = beforeCondition && TriggerSystem.trigBarHide;
        TargetScale = button.enabled ? Vector3.one : Vector3.zero;

        float tmpProgress = Mathf.Clamp(progress, 0f, 1f);

        transform.localScale = LineAnimation.Lerp(transform.localScale, TargetScale, tmpProgress, 0.5f, 0.5f);
        progress += Time.deltaTime * 0.5f;
    }
}
