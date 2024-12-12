using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public string Name;
    public string Description; 
    public int Index;
}
