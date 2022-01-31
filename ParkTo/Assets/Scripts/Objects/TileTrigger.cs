using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int index)
    {
        spriteRenderer.sprite = TriggerSystem.instance.triggerSprites[index];
    }
}
