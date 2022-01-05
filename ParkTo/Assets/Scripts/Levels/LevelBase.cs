using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Name", menuName = "Level")]
public class LevelBase : ScriptableObject
{
    public string id;
    public string description;

    public int theme;
    public Vector2Int size;

    public string grounds;
    public string groundRotations;

    public string line;
    public string lineRotations;

    public string cars;
    public string carRotations;

    public string triggers;
    public string triggerInformations;

    public int seed;

    public int[,] ToArray(string data)
    {
        int[,] result = new int[size.y, size.x];
        string[] splitData = data.Split(',');

        for(int y = 0; y < size.y; y++)
            for(int x = 0; x < size.x; x++)
                result[y, x] = int.Parse(splitData[y * size.x + x]);

        return result;
    }
}
