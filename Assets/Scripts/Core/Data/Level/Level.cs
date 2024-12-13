using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public new string name;
    public string description; 
    public int index;
}
