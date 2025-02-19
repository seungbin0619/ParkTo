using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 현재 플레이하는 레벨이 뭔지 저장하는 클래스
/// </summary>
[DisallowMultipleComponent]
public class LevelGenerator : MonoBehaviour {
    public event Action<LevelGenerator> OnLevelGenerated = delegate {};
    
    private Level _level;
    public Grid Grid { get; private set; }
    public IEnumerable<Car> Cars { get; private set; }
    public Triggers Triggers { get; private set; }
    public Rect ViewRect => _level.Rect;

    public int LevelIndex { get; private set; }
    public LevelPack LevelPack { get; private set; }
    
    public bool HasInitialized => _level != null;

    public void Initialize(LevelPack levelPack, int index) {
        LevelPack = levelPack;
        _level = LevelPack.levels[index];

        Grid = new(_level.grounds.Select(g => new Ground(g)), _level.Rect);
        Cars = _level.cars.Select(c => new Car(c, Grid[c.position]));
        
        Triggers = new(_level.triggers);
    
        LevelIndex = index;
        _level.Initialize();

        OnLevelGenerated?.Invoke(this);
    }
}