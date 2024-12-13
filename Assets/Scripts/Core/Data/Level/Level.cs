using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public new string name;
    public string description; 
    public int index;
    public List<CarSerilizer> cars;
    public List<TriggerSerilizer> triggers;

    // public List<GameObject> decorators;
}

[Serializable]
public struct CarSerilizer {
    public Color color;
    public Vector2Int size;
    public Vector2Int position;
    public Direction direction;
}

[Serializable]
public struct GroundSerilizer {
    public Vector2Int position;
    
}

[Serializable]
public struct TriggerSerilizer {
    public int count;
    public Trigger.Type type;
}