using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    public void OnTriggerStateChanged()
    {
        text.text = !TriggerSystem.instance.triggerMode ?
            "트리거를 설치할 타일을 선택해주세요." : "트리거를 적용할 차를 선택해주세요.";
    }
}
