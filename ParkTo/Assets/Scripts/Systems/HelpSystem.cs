using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static HelpSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    #endregion


}
