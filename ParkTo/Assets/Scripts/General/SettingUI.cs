using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public void GotoSelect()
    {
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Select");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }

    public void GotoTitle()
    {
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Title");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }
}
