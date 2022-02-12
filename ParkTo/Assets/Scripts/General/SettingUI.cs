using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Slider bgm;

    [SerializeField]
    private UnityEngine.UI.Slider sound;

    public void Awake()
    {
        bgm.value = DataSystem.GetData("Setting", "Bgm", 50) * 0.01f;
        sound.value = DataSystem.GetData("Setting", "Sound", 50) * 0.01f;
    }

    public void OnBGMChanged()
    {
        DataSystem.SetData("Setting", "Bgm", (int)(bgm.value * 100));

        SFXSystem.instance.OnSoundChange();
    }

    public void OnSoundChanged()
    {
        DataSystem.SetData("Setting", "Sound", (int)(sound.value * 100));
    }


    public void GotoSelect()
    {
        DataSystem.SaveData();

        SFXSystem.instance.PlaySound(3);

        if (LevelSystem.instance.SelectedLevel > 0)
            LoadSelect.tmpIndex = LevelSystem.instance.SelectedLevel % 8;

        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Select");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();

        MapSystem.CurrentLevel = null;
    }

    public void GotoTitle()
    {
        DataSystem.SaveData();

        SFXSystem.instance.PlaySound(3);

        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Title");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }
}
