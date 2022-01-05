using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private int targetIndex = -1;
    private Vector3Int position;

    private SpriteRenderer spriteRenderer;
    public bool IsArrived
    {
        get
        {
            if (targetIndex == -1) return false;
            return position == MapSystem.CurrentCars[targetIndex].position;
        }
    }
    private bool beforeCondition = false;
    private Vector3 targetScale = Vector3.one;
    private float progress = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector3Int position, int targetIndex)
    {
        this.position = position;
        this.targetIndex = targetIndex;
        spriteRenderer.color = MapSystem.CurrentCars[targetIndex].color;
    }

    private void Update()
    {
        if (targetIndex == -1) return;

        if (beforeCondition != IsArrived)
        {
            beforeCondition = IsArrived;
            targetScale = beforeCondition ? Vector3.zero : Vector3.one;

            progress = 0;
        }

        float tmpProgress = Mathf.Clamp(progress, 0f, 1f);
        transform.localScale = LineAnimation.Lerp(transform.localScale, targetScale, tmpProgress, 0.5f, 0.5f);

        progress += Time.deltaTime * 0.5f;
    }
}
