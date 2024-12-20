using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public new string name;
    public string description; 
    public int index;
    public Rect Rect { get; private set; }

    public List<GroundSerializer> grounds;
    public List<CarSerializer> cars;
    public List<TriggerSerializer> triggers;

    void OnValidate() {
        InitializeRect();
    }

    private void InitializeRect() {
        if(grounds.Count == 0) return;

        int xMin, zMin, xMax, zMax;
        xMin = xMax = grounds[0].position.x;
        zMin = zMax = grounds[0].position.z;

        for(int i = 1; i < grounds.Count; i++) {
            xMin = Mathf.Min(xMin, grounds[i].position.x);
            xMax = Mathf.Max(xMax, grounds[i].position.x);
            zMin = Mathf.Min(zMin, grounds[i].position.z);
            zMax = Mathf.Max(zMax, grounds[i].position.z);
        }

        Rect = new(xMin, zMin, xMax - xMin + 1, zMax - zMin + 1);
    }

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
    public TriggerType trigger;
    
    public GroundSerializer(Point position, TriggerType trigger) {
        this.position = position;
        this.trigger = trigger;
    }
}

[Serializable]
public struct TriggerSerializer {
    public int count;
    public TriggerType type;
}