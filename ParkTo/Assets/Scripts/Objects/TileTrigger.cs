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
        if(index == -2) {
            spriteRenderer.sprite = TriggerSystem.instance.banTrigger;
            return;
        }

        spriteRenderer.sprite = TriggerSystem.instance.triggerSprites[index];
    }
}
