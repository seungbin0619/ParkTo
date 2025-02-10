#pragma warning disable IDE1006

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class ViewInputModule {
    protected override string TargetScene => "LevelObjectView";

    public event Action<IAssignableView> OnSelectedViewChanged = delegate {};
    private static readonly List<ViewInputModule> _viewInputSystems = new();
    public static ViewInputModule current => _viewInputSystems.FirstOrDefault();

    private LevelInputModule _module;
    private LevelView _view;
    private LevelGenerator _generator;
    private List<IAssignableView> _currentViews;

    protected override void Awake()
    {
        base.Awake();
        
        if(!Application.isPlaying) return;
        var levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        
        _view = levelManager.GetComponent<LevelView>();
        _generator = levelManager.GetComponent<LevelGenerator>();

        _module = GetComponentInParent<LevelInputModule>();
    }

    public override void Select(IAssignableView view) {
        if(!HasValue(_selected)) Select();

        base.Select(view);
        OnSelectedViewChanged?.Invoke(view);
    }

    private void OnDrawGizmos() {
        if(_selected == null) return;

        Gizmos.color = _submitted == null ? Color.red : Color.blue;
        Gizmos.DrawSphere(_selected.transform.position, 1f);
    }
}

public partial class ViewInputModule : SelectableList<IAssignableView>
{
    public override void OnSubmitted()
    {
        base.OnSubmitted();
        _module.AssignTrigger();  
    }

    public override void OnSelect(BaseEventData eventData)
    {
        OnMoveInput(GetLastInput());
        
        base.OnSelect(eventData);
    }

    private Direction GetLastInput() {
        BaseInputModule module = EventSystem.current.currentInputModule;
        if(!module) return Direction.None;
        
        float dx = module.input.GetAxisRaw("Horizontal");
        float dy = module.input.GetAxisRaw("Vertical");

        if(dx == 0 && dy == 0) return Direction.None;

        if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
            return dx > 0 ? Direction.Right : Direction.Left;
        } else {
            return dy > 0 ? Direction.Up : Direction.Down;
        }
    }

    public override void OnMove(AxisEventData eventData)
    {
        Direction direction = eventData.moveVector.ToDirection();
        if(!OnMoveInput(direction)) return;

        base.OnMove(eventData);
    }

    private bool OnMoveInput(Direction direction) {
        if(direction == Direction.None) return true;

        if(_selected == null) {
            Select(GetBoundaryViewInDirection(direction));
            return true;
        }

        var IsPositive = direction.IsPositive();
        var swap = direction.Swap();
        
        // view의 rotate를 반영한 direction으로 변환
        var grid = _selected.transform.parent.parent;
        direction = grid.InverseTransformDirection(direction.ToPoint()).ToDirection();
        swap = grid.InverseTransformDirection(swap.ToPoint()).ToDirection();

        Point mask = new() {
            x = Math.Sign(direction.ToPoint().z),
            z = Math.Sign(direction.ToPoint().x)
        };

        // 같은 위치 요소(Car 등으로 인한..)를 포함할지 여부 결정
        Point targetPoint = _selected.position;
        if(IsPositive == (_selected.GetType() == typeof(CarView))) 
            targetPoint += direction.ToPoint();

        var rect = _generator.Grid.Rect;
        var inlineViews = _currentViews.Where(v => v != _selected && (v.position * mask == targetPoint * mask));
        
        while(inlineViews.All(view => view.position != targetPoint)) {
            if(!rect.Contains((Vector2)targetPoint)) { // 범위 밖으로 나간 경우에는
                targetPoint += swap.ToPoint();
                targetPoint += direction.Opposite().ToPoint();

                if(!rect.Contains((Vector2)targetPoint)) {
                    inlineViews = null;
                    break;
                }

                inlineViews = _currentViews.Where(v => v != _selected && (v.position * mask == targetPoint * mask));
                var first = inlineViews.OrderByDescending(view => (view.position.x + view.position.z) * (mask.x + mask.z)).LastOrDefault();
                targetPoint = first.position;

            } else targetPoint += direction.ToPoint();
        }

        if(IsPositive) Select(inlineViews?.FirstOrDefault(view => view.position == targetPoint) ?? _selected);
        else Select(inlineViews?.LastOrDefault(view => view.position == targetPoint));
        
        return _selected == null;
    }

    public IAssignableView GetBoundaryViewInDirection(Direction direction) {
        if(direction == Direction.None) return null;

        return _currentViews.OrderByDescending(view =>
            Vector3.Dot(direction.Opposite().ToPoint(), Camera.main.WorldToViewportPoint(view.transform.position)))
            .FirstOrDefault();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Select(null);

        base.OnDeselect(eventData);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(!Application.isPlaying) return;
        
        _viewInputSystems.Add(this);
        _view.OnViewCreated += Initialize;
    }
    
    private void Initialize() {
        _currentViews = new();
        
        _currentViews.AddRange(_view.GroundViews.Values);
        _currentViews.AddRange(_view.CarViews);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(!Application.isPlaying) return;

        _viewInputSystems.Remove(this);
        _view.OnViewCreated -=Initialize;
    }

#if UNITY_EDITOR
    protected override void Reset() {
        base.Reset();

        transition = Transition.None;
        Navigation navigation = new()
        {
            mode = Navigation.Mode.Explicit | Navigation.Mode.Automatic
        };
        
        this.navigation = navigation;
    }
#endif
}
