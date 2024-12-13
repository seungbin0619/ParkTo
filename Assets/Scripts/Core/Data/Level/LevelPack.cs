using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPack", menuName = "Level/Level Pack")]
public class LevelPack : ScriptableObject
{
    public new string name;
    public string description; 
    public int index;
    public List<Level> levels;
    public LevelStyle style;
}
