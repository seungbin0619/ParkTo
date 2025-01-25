using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 레벨 상태 정보들을 총체적으로 담는 클래스.
/// ex) 클리어 여부, 차, 트리거, ...
/// </summary>
[RequireComponent(typeof(LevelView))]
[RequireComponent(typeof(LevelAction))]
[RequireComponent(typeof(LevelGenerator))]
public partial class LevelState : MonoBehaviour {
    private LevelGenerator _generator;
    private LevelAction _action;
    private LevelView _view;

    public bool IsPlaying => !_view.CarViews.Any(view => view.IsAnimating);
    
    void Awake() {
        _generator = GetComponent<LevelGenerator>();
        _action = GetComponent<LevelAction>();
        _view = GetComponent<LevelView>();
    }

    public void Initialize(LevelPack levelPack, int index) {
        if(index < 0 || index >= levelPack.levels.Count) {
            return;
        }

        _generator.Initialize(levelPack, index);

        _view.Initialize(levelPack.style);
        _view.CreateView();
    }
}