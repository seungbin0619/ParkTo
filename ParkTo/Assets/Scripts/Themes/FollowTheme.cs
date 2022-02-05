using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheme : MonoBehaviour
{
    private UnityEngine.UI.Image image;
    [SerializeField]
    private int index = -1;
    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void Start()
    {
        if (ThemeSystem.CurrentTheme != null)
            OnThemeChanged();
    }

    public void OnThemeChanged()
    {
        image.color = ThemeSystem.CurrentTheme.colors[index];
    }
}
