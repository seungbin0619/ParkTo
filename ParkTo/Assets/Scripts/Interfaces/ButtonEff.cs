using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEff : MonoBehaviour
{
    private Image image;
    private Vector3 targetScale = Vector3.one;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image.rectTransform.localScale = Vector3.one;
        targetScale = Vector3.one;
    }

    private void Update()
    {
        image.rectTransform.localScale = Vector3.Lerp(image.rectTransform.localScale, targetScale, Time.deltaTime * 10f);
    }

    public void TriggerPointerEnter(BaseEventData e)
    {
        targetScale = Vector3.one * 1.2f;
    }

    public void TriggerPointerExit(BaseEventData e)
    {
        targetScale = Vector3.one;
    }
}
