using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPack", menuName = "Level/Level Pack")]
public class LevelPack : ScriptableObject
{
    public string Name;
    public string Description; 
    public int Index;
    public List<Level> Levels;
    public LevelStyle Style;
}
