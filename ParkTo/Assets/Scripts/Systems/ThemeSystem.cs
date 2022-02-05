using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static ThemeSystem instance;

    private void Awake()
    {
        if (instance == null) instance = this;

        DontDestroyOnLoad(this);
    }

    #endregion

    public ThemeBase[] themes;
    public static ThemeBase CurrentTheme;

    private void Start()
    {
        LoadTheme();
    }

    private void LoadTheme()
    {
        int theme = DataSystem.GetData("Setting", "Theme", 0);
        SetTheme(theme);
    }

    public void SetTheme(int index)
    {
        CurrentTheme = themes[index];

        Vars.instance.OnThemeChanged.Raise();
    }
}
