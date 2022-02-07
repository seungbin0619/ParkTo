using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    void Start()
    {
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Move, "Title");
        ActionSystem.instance.AddAction(ActionSystem.Action.ActionType.Fade, 0);

        ActionSystem.instance.Play();
    }
}
