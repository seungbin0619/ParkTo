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
    public void SetCanvas(Canvas canvas)
    {
        CurrentCanvas = canvas;
    }

    public void OpenSetting()
    {
        if (CurrentCanvas == null) return;

        CurrentCanvas.gameObject.SetActive(true);
    }

    public void CloseSetting()
    {
        if (CurrentCanvas == null) return;

        CurrentCanvas.gameObject.SetActive(false);
    }
}
