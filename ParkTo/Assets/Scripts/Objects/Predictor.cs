using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predictor : MonoBehaviour
{
    private static readonly Vector3 adjust = new Vector3(0.5f, 0.5f);
    private SpriteRenderer spriteRenderer;

    private Color color;
    public int index = -1;

    private bool enable = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color32 color, int index, Vector3 position, bool middle = false)
    {
        color.a = 0;

        this.color = spriteRenderer.color = color;
        transform.localPosition = position + adjust;
        if (middle) transform.localScale = Vector3.one * 0.25f;

        this.index = index;
    }

    private void Update()
    {
        if (!enable) return;
        if (index == -1) return;

        color.a = Mathf.Sin((Time.time - index * 0.166f) * Mathf.PI);
        spriteRenderer.color = color;
    }

    public void SetEnable(bool value)
    {
        enable = value;
        if (!enable)
        {
            color.a = 0;
            spriteRenderer.color = color;
        }
    }
}
