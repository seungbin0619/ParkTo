using UnityEngine;

[CreateAssetMenu(fileName = "LevelStyle", menuName = "Level/Level Style")]
public class LevelStyle : ScriptableObject
{
    public RuleTile groundTile;
    public CarView carView;

    public Color primaryColor = Color.white;
    public Color secondaryColor = Color.white;
}
