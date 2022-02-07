using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Theme Name", menuName = "Theme")]
public class ThemeBase : ScriptableObject
{
    public new string name;
    public int index;

    public Color32[] colors;
    public Color32[] carColors;

    public Tile[] grounds;
    public Tile[] outlines;
    public Tile[] decorates;
}
