using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 레벨 상태 정보들을 총체적으로 담는 클래스.
/// ex) 클리어 여부, 차, 트리거, ...
/// </summary>
public class LevelState : MonoBehaviour {
    private LevelView _levelView;
    private List<CarView> _carViews;
    private bool _isPlaying;

    public IEnumerable<CarView> CarViews => _carViews;

    void Awake() {
        _levelView = GetComponent<LevelView>();
    }

    public bool IsPlaying() {
        return _isPlaying && CarViews.Where(view => !view.Car.Variables.isStop).Count() > 0;
    }
}