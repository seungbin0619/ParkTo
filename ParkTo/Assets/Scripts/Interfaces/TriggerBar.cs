using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerBar : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static TriggerBar instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        rect = GetComponent<RectTransform>();

        targetPosition = barPosition[1];
    }

    #endregion

    private readonly Vector2[] barPosition = new Vector2[2]
    {
        new Vector2(0, 40f), new Vector2(0, 160f)
    };

    private bool _isHide = false;
    public bool IsHide
    {
        get { return _isHide; }
        set
        {
            if (TriggerSystem.instance.selectedTrigger != null) return;
            _isHide = value;

            targetPosition = barPosition[_isHide ? 0 : 1];
            TriggerSystem.trigBarHide = _isHide;
        }
    }
    private RectTransform rect;
    private Vector2 targetPosition;

    public Vector3 targetSizeDelta;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (rect.position.y >= mousePosition.y)
        {
            if (IsHide) IsHide = false;
        }else if(!IsHide)
        {
            if(Input.GetMouseButtonDown(0)) // 바깥 부분 클릭하면
                IsHide = true;
        }

        rect.sizeDelta = Vector3.Lerp(rect.sizeDelta, targetSizeDelta, Time.deltaTime * 5f);

        if (MapSystem.MoveFlag) return;

        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPosition, Time.deltaTime * 5f);
    }
}
