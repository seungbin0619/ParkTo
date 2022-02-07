using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowText : MonoBehaviour
{
    private TMPro.TMP_Text text;
    [SerializeField]
    private int index = -1;
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    public void Start()
    {
        if (ThemeSystem.CurrentTheme != null)
            OnThemeChanged();
    }

    public void OnThemeChanged()
    {
        text.color = ThemeSystem.CurrentTheme.colors[index];
    }
}
