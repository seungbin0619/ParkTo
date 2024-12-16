using System.Collections.Generic;
using System.Linq;
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
    public IEnumerable<CarView> CarViews => _carViews;

    void Awake() {
        _generator = GetComponent<LevelGenerator>();
    }

    public void Initialize(LevelStyle style) {
        _style = style;
    }

    public void CreateView() {
        DestroyView();

        MoveViewToCenter();
        
        InstantiateGroundViews();
        InstantiateCarViews();
    }

    public void DestroyView() {
        foreach(var view in _carViews) Destroy(view);
        _carViews.Clear();

        foreach(var view in _groundViews.Values) Destroy(view);
        _groundViews.Clear();
    }

    private void MoveViewToCenter() {
        Rect rect = _generator.ViewRect;
        Vector3 position = -rect.position - (rect.size - Vector2.one) * 0.5f;

        groundTile.transform.position = position.XZY();
    }

    private void InstantiateCarViews() {
        foreach(var car in _generator.Cars) {
            var view = Instantiate(_style.carView, groundTile.transform); // add parent

            view.Initialize(car);
            _carViews.Add(view);
        }
    }

    private void InstantiateGroundViews() {
        foreach(var ground in _generator.Grid.Grounds) {
            Vector3Int position = ground.Position;
            position = position.XZY();

            groundTile.SetTile(position, _style.groundTile);

            var view = groundTile.GetInstantiatedObject(position).GetComponent<GroundView>();
            view.Initialize(ground);
            
            _groundViews.Add(ground.Position, view);
        }
    }
}