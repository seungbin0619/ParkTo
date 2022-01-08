using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private UnityEngine.UI.Button button;

    private bool beforeCondition = true;
    private Vector3 targetScale = Vector3.one;
    private float progress = 0;

    public void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    public void PrevMove()
    {
        button.enabled = false;

        beforeCondition = false;
        targetScale = Vector3.zero;
        progress = 0;
    }

    public void AfterChange()
    {
        if (beforeCondition != MapSystem.isPlayable)
        {
            button.enabled = MapSystem.isPlayable;
            beforeCondition = MapSystem.isPlayable;
            targetScale = beforeCondition ? Vector3.one : Vector3.zero;
            progress = 0;
        }
    }

    public void Update()
    {
        if (transform.localScale == targetScale) return;
        float tmpProgress = Mathf.Clamp(progress, 0f, 1f);

        transform.localScale = LineAnimation.Lerp(transform.localScale, targetScale, tmpProgress, 0.5f, 0.5f);
        progress += Time.deltaTime * 0.5f;
    }
}
