using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vars : MonoBehaviour
{
    #region 인스턴스

    public static Vars instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    #endregion

    public GameEvent AfterMove;
    public GameEvent OnChanged;
    public GameEvent OnThemeChanged;
}
