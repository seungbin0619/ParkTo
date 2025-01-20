using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 현재 플레이하는 레벨이 뭔지 저장하는 클래스
/// </summary>
public class LevelGenerator : MonoBehaviour {
    public static UnityEvent<Level> onLevelGenerated = new();
    private Level _level;
    private LevelPack _pack;
    public Grid Grid { get; private set; }
    public IEnumerable<Car> Cars { get; private set; }
    public Rect ViewRect => _level.Rect;

    public int LevelIndex { get; private set; }

    public void Initialize(LevelPack levelPack, int index) {
        _pack = levelPack;
        _level = _pack.levels[index];

        Grid = new(_level.grounds.Select(g => new Ground(g)), _level.Rect);
        Cars = _level.cars.Select(c => new Car(c, Grid[c.position]));
    
        LevelIndex = index;
        _level.Initialize();
        
        onLevelGenerated?.Invoke(_level);
    }
}