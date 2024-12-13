using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public new string name;
    public string description; 
    public int index;

    public List<GroundSerializer> grounds;
    public List<CarSerializer> cars;
    public List<TriggerSerializer> triggers;

    // public List<GameObject> decorators;
}

[Serializable]
public struct CarSerializer {
    public Color color;
    public Point size;
    public Point position;
    public Direction direction;
}

[Serializable]
public struct GroundSerializer {
    public Point position;
    
}

[Serializable]
public struct TriggerSerializer {
    public int count;
    public Trigger.Type type;
}