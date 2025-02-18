using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/// <summary>
/// 레벨에 필요한 View들을 생성하고 관리하는 클래스
/// </summary>
[DisallowMultipleComponent]
public class LevelView : MonoBehaviour {
    public event Action OnViewCreated = delegate {};
    public event Action OnViewDestroyed = delegate {};

    private LevelStyle _style;
    private LevelGenerator _generator;
    
    [SerializeField] 
    private Tilemap groundTile;

    // test
    private Transform _gridTransform;
    private int _gridRotation = 0;
    //

    private readonly List<CarView> _carViews = new();
    private readonly Dictionary<Point, GroundView> _groundViews = new();
    
    public IEnumerable<CarView> CarViews => _carViews;
    public IDictionary<Point, GroundView> GroundViews => _groundViews;

    void Awake() {
        _generator = GetComponent<LevelGenerator>();
        _gridTransform = groundTile.transform.parent;
    }

    public void Initialize(LevelStyle style) {
        _style = style;
    }

    public void CreateView() {
        DestroyView();
        
        InitializeGridView();
        InstantiateGroundViews();
        InstantiateCarViews();

        OnViewCreated?.Invoke();
    }

    public void DestroyView() {
        foreach(var view in _carViews) Destroy(view.gameObject);

        _carViews.Clear();

        foreach(var view in _groundViews.Values) 
            Destroy(view.gameObject);

        _groundViews.Clear();
        groundTile.ClearAllTiles();
        
        OnViewDestroyed?.Invoke();
    }

    private void InitializeGridView() {
        Rect rect = _generator.ViewRect;
        Vector3 position = -rect.position - (rect.size - Vector2.one) * 0.5f;

        _gridRotation = 0;
        _gridTransform.transform.rotation = Quaternion.identity;
        groundTile.transform.localPosition = position.XZY();
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
            position = position.XZY(); // clean up

            groundTile.SetTile(position, _style.groundTile);

            var view = groundTile.GetInstantiatedObject(position).GetComponent<GroundView>();
            view.Initialize(ground);
            
            _groundViews.Add(ground.Position, view);
        }
    }

    // test
    public void RotateLevelView(int direction) {
        Vector3 rotation = _gridTransform.eulerAngles;
        _gridRotation += direction + 4;
        _gridRotation %= 4;

        rotation.y = 90 * _gridRotation;

        _gridTransform.DORotate(rotation, 0.5f).SetEase(Ease.OutCubic);
    }
}