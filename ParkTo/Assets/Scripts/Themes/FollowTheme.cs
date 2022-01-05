using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheme : MonoBehaviour
{
    private UnityEngine.UI.Image image;
    [SerializeField]
    private int index = -1;
    private void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void OnThemeChanged()
    {
        image.color = MapSystem.CurrentTheme.colors[index];
    }
}
