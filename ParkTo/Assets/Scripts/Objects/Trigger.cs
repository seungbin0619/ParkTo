using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trigger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [SerializeField]
    private Image image;

    public int index;
    private Vector3 targetScale = Vector3.one;
    private ScrollRect parentRect;

    private void Awake()
    {
        //image = GetComponent<UnityEngine.UI.Image>();
        
    }

    public void Initialize(int index, ScrollRect parent)
    {
        this.index = index;

        image.sprite = TriggerSystem.instance.triggerSprites[index];
        parentRect = parent;
    }

    public void Select()
    {
        //Debug.Log("test");

        TriggerSystem.instance.Select(this);
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

    #region [ 이벤트 전달 ]

    public void OnBeginDrag(PointerEventData e)
    {
        parentRect.OnBeginDrag(e);
    }
    public void OnDrag(PointerEventData e)
    {
        parentRect.OnDrag(e);
    }
    public void OnEndDrag(PointerEventData e)
    {
        parentRect.OnEndDrag(e);
    }

    public void OnScroll(PointerEventData e)
    {
        parentRect.OnScroll(e);
    }

    #endregion
}
