using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predictor : MonoBehaviour
{
    private static readonly Vector3 adjust = new Vector3(0.5f, 0.5f);
    private SpriteRenderer spriteRenderer;
    private int index;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color32 color, int index, Vector3 position, bool middle = false)
    {
        spriteRenderer.color = color;
        transform.localPosition = position + adjust;

        this.index = index;
    }
}
