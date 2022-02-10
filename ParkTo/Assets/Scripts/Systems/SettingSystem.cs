using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static SettingSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    #endregion

    private Canvas CurrentCanvas;
    public static bool IsOpen;

    private UnityEngine.UI.Image hide;
    private RectTransform border;

    private const float duration = 0.5f;
    public static bool isAnimate;

    public void SetCanvas(Canvas canvas)
    {
        IsOpen = false;
        isAnimate = false;

        CurrentCanvas = canvas;

        hide = canvas.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        border = canvas.transform.GetChild(1).GetComponent<RectTransform>();
    }

    public void OpenSetting()
    {
        if (isAnimate) return;
        if (CurrentCanvas == null) return;
        IsOpen = true;

        CurrentCanvas.gameObject.SetActive(true);
        StartCoroutine(CoSetting(true));
    }

    public void CloseSetting()
    {
        if (isAnimate) return;
        if (CurrentCanvas == null) return;
        IsOpen = false;

        StartCoroutine(CoSetting(false));
    }

    private IEnumerator CoSetting(bool flag)
    {
        isAnimate = true;
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        float progress = 0f;

        Vector2 currentPosition = border.anchoredPosition, targetPosition = currentPosition;
        Color currentColor = hide.color, targetColor = currentColor;

        float height = border.sizeDelta.y;

        targetPosition.y = 50 + (flag ? 0 : height);
        targetPosition *= flag ? 1 : -1;
        targetColor.a = flag ? 0.4f : 0;

        while(true)
        {
            float clamp = progress / duration;
            clamp = LineAnimation.Lerp(0, 1, clamp, 1, flag ? 0 : 1, flag ? 1 : 0);
            border.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, clamp);
            hide.color = Color.Lerp(currentColor, targetColor, clamp);

            yield return delay;
            progress += Time.deltaTime;

            if (progress > duration) break;
        }

        CurrentCanvas.gameObject.SetActive(flag);
        isAnimate = false;
    }
}
