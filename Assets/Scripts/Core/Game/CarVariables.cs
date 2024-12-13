using System;
using UnityEngine;

[Serializable]
public class CarVariables {
    public Point position;
    [Range(0, 4)] public Direction direction;

    [HideInInspector] public float speed; // speed: ground per second
    [HideInInspector] public bool isStop;
    [HideInInspector] public bool isBackUp; 
    [HideInInspector] public bool isBroken;

    public CarVariables(CarVariables variables) {
        position = variables.position;
        direction = variables.direction;
        speed = variables.speed;
        isStop = variables.isStop;
        isBackUp = variables.isBackUp;
        isBroken = variables.isBroken;
    }

    public CarVariables(Point position, Direction direction, float speed = 1f, bool isStop = false, bool isBackUp = false, bool isBroken = false) {
        this.position = position;
        this.direction = direction;
        this.speed = speed;
        this.isStop = isStop;
        this.isBackUp = isBackUp;
        this.isBroken = isBroken;
    }

    public CarVariables(CarSerializer serilizer) {
        position = serilizer.position;
        direction = serilizer.direction;

        speed = 1f;
        isStop = false;
        isBackUp = false;
        isBroken = false;
    }

    public void Reset() {
        speed = 1f;
        isStop = false;
        isBackUp = false;
        isBroken = false;
    }

    public void Rotate(int dir) {
        direction = direction.Rotated(dir);
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