using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public void OpenHelp()
    {
        int index = 0;
        if (MapSystem.CurrentLevel != null) index = MapSystem.CurrentLevel.help;

        HelpSystem.instance.OpenCanvas(index);
    }
}
