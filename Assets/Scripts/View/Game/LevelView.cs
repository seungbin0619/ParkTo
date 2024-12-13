using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 레벨에 필요한 View들을 생성하고 관리하는 클래스
/// </summary>
public class LevelView : MonoBehaviour {
    private LevelStyle _style;
    private LevelGenerator _generator;
    [SerializeField] private Tilemap groundTile;

    private readonly List<CarView> _carViews = new();
    private readonly Dictionary<Point, GroundView> _groundViews = new();

    void Awake() {
        _generator = GetComponent<LevelGenerator>();
    }

    public void Initialize(LevelStyle style) {
        _style = style;
    }

    public void CreateView() {
        DestroyView();

        InstantiateGroundViews();
        InstantiateCarViews();
    }

    public void DestroyView() {
        foreach(var view in _carViews) Destroy(view);
        _carViews.Clear();

        foreach(var view in _groundViews.Values) Destroy(view);
        _groundViews.Clear();
    }

    private void InstantiateCarViews() {
        foreach(var car in _generator.Cars) {
            var view = Instantiate(_style.carView); // add parent

            view.Initialize(car);
            _carViews.Add(view);
        }
    }

    private void InstantiateGroundViews() {
        foreach(var ground in _generator.Grid.Grounds) {
            groundTile.SetTile(ground.Position, _style.groundTile);

            var view = groundTile.GetTile(ground.Position).GetComponent<GroundView>();
            view.Initialize(ground);
            
            _groundViews.Add(ground.Position, view);
        }
    }
}