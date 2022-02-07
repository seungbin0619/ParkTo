using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingText : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }
    void Start()
    {
        SetText();

    }

    private void SetText()
    {
        if (MapSystem.CurrentLevel != null)
            text.text = MapSystem.CurrentLevel.id;
    }

    public void OnLevelLoaded()
    {
        SetText();
    }
}
