using UnityEngine;

/// <summary>
/// 현재 플레이하는 레벨이 뭔지 저장하는 클래스
/// </summary>
public class LevelData : MonoBehaviour {
    private Level _level;
    private LevelPack _pack;
    private LevelStyle _style;
    public int LevelIndex { get; private set; }

    public void Initialize(LevelPack levelPack, int index) {
        if(index < 0 || index >= _pack.levels.Count) {
            return;
        }

        _pack = levelPack;
        _style = _pack.style;
        _level = _pack.levels[index];
        
        LevelIndex = index;
    }
}