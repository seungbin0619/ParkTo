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
    private Grid _grid;
    public Grid Grid => _grid ??= new(_level.grounds.Select(g => new Ground(g)) ?? Enumerable.Empty<Ground>(), _level.Rect);
    public IEnumerable<Car> Cars => _level.cars.Select(c => new Car(c, Grid[c.position])) ?? Enumerable.Empty<Car>();
    public Rect ViewRect => _level.Rect;

    public int LevelIndex { get; private set; }

    public void Initialize(LevelPack levelPack, int index) {
        _pack = levelPack;
        _level = _pack.levels[index];
        LevelIndex = index;

        onLevelGenerated?.Invoke(_level);
    }
}