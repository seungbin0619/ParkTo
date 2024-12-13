using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 현재 플레이하는 레벨이 뭔지 저장하는 클래스
/// </summary>
public class LevelGenerator : MonoBehaviour {
    private Level _level;
    private LevelPack _pack;
    public IEnumerable<Car> Cars => _level.cars.Select(c => new Car(c)) ?? Enumerable.Empty<Car>();
    public Grid Grid => new(_level.grounds.Select(g => new Ground(g)) ?? Enumerable.Empty<Ground>());

    public int LevelIndex { get; private set; }

    public void Initialize(LevelPack levelPack, int index) {
        _pack = levelPack;
        _level = _pack.levels[index];
        
        LevelIndex = index;
    }
}