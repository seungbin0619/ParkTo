using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 레벨 상태 정보들을 총체적으로 담는 클래스.
/// ex) 클리어 여부, 차, 트리거, ...
/// </summary>
public class LevelState : MonoBehaviour {
    private LevelData _levelData;
    private LevelView _levelView;

    void Awake() {
        _levelData = GetComponent<LevelData>();
        _levelView = GetComponent<LevelView>();
    }

    public void Initialize() {
        
    }
}