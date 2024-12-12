using System;
using UnityEngine;

[Serializable]
public struct CarVariables {
    public Vector2Int position;
    [Range(0, 4)] public Direction direction;

    [HideInInspector] public float speed; // speed: ground per second
    [HideInInspector] public bool isStop;
    [HideInInspector] public bool isBackUp; 
    [HideInInspector] public bool isBroken;

    public void Reset() {
        speed = 1f;
        isStop = false;
        isBackUp = false;
        isBroken = false;
    }

    public void Translate(Vector2Int position) {
        this.position = position;
    }

    public void Rotate(int dir) {
        direction = direction.Rotated(dir);
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void Stop() {
        isStop = true;
    }

    public void BackUp() {
        isBackUp = true;
    }

    public void Broke() {
        isBroken = true;
    }
}