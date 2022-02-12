using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public void StartGame()
    {
        //DataSystem.Load(true);
        SFXSystem.instance.PlaySound(3);

        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 1);
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Select");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
