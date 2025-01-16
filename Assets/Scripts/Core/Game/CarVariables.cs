using System;
using UnityEngine;

[Serializable]
public class CarVariables {
    public Point position;
    [Range(0, 4)] public Direction direction;
    [HideInInspector] public float speed; // speed: ground per second
    [HideInInspector] public bool isStart;
    [HideInInspector] public bool isStop;
    [HideInInspector] public bool isBackUp; 
    [HideInInspector] public bool isBroken;

    public CarVariables(CarVariables variables) {
        position = variables.position;
        direction = variables.direction;
        speed = variables.speed;
        isStart = false;
        isStop = variables.isStop;
        isBackUp = variables.isBackUp;
        isBroken = variables.isBroken;
    }

    public CarVariables(Point position, Direction direction, float speed = 1f, bool isStart = false, bool isStop = false, bool isBackUp = false, bool isBroken = false) {
        this.position = position;
        this.direction = direction;
        this.speed = speed;
        this.isStart = isStart;
        this.isStop = isStop;
        this.isBackUp = isBackUp;
        this.isBroken = isBroken;
    }

    public CarVariables(CarSerializer serilizer) {
        position = serilizer.position;
        direction = serilizer.direction;

        speed = 1f;
        isStart = true;
        isStop = false;
        isBackUp = false;
        isBroken = false;
    }

    public Direction GetDirection() {
        return isBackUp ? direction.Opposite() : direction;
    }

    public CarVariables Next() {
        CarVariables next = new(this) {
            position = position.Next(GetDirection())
        };

        return next;
    }

    public void Reset() {
        speed = 1f;
        isStart = true;
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
    
    public override string ToString()
    {
        return $"Position: {position}\nDirection: {direction}\nSpeed: {speed}\nIsStart: {isStart}\nIsStop: {isStop}\nIsBackUp: {isBackUp}\nIsBroken: {isBroken}";
    }

}