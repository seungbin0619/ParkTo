using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetting : ButtonEff
{
    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        SettingSystem.instance.SetCanvas(canvas);
    }

    public void OpenSetting()
    {
        SettingSystem.instance.OpenSetting();
    }
}
