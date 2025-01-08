using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class ViewInputSystem {
    [SerializeField] 
    private LevelView _view;

    [SerializeField]
    private LevelGenerator _generator;
    
    private List<IAssignableView> _currentViews;
    private IAssignableView selectedView = null;
    
    private void Initialize() {
        _currentViews = new();
        
        _currentViews.AddRange(_view.GroundViews.Values);
        _currentViews.AddRange(_view.CarViews);
    }

    // return cancel flag
    private bool OnMoveInput(Direction direction) {
        if(direction == Direction.None) return true;

        if(selectedView == null) {
            selectedView = GetBoundaryViewInDirection(direction);
            return true;
        }

        var IsPositive = direction.IsPositive();
        var swap = direction.Swap();
        
        // view의 rotate를 반영한 direction으로 변환
        var grid = selectedView.transform.parent.parent;
        direction = grid.InverseTransformDirection(direction.ToPoint()).ToDirection();
        swap = grid.InverseTransformDirection(swap.ToPoint()).ToDirection();

        Point mask = new() {
            x = Math.Sign(direction.ToPoint().z),
            z = Math.Sign(direction.ToPoint().x)
        };

        // 같은 위치 요소(Car 등으로 인한..)를 포함할지 여부 결정
        Point targetPoint = selectedView.position;
        if(IsPositive == (selectedView.GetType() == typeof(CarView))) 
            targetPoint += direction.ToPoint();

        //
        
        var rect = _generator.Grid.Rect;
        var inlineViews = _currentViews.Where(v => v != selectedView && (v.position * mask == targetPoint * mask));
        
        while(inlineViews.All(view => view.position != targetPoint)) {
            if(!rect.Contains((Vector2)targetPoint)) { // 범위 밖으로 나간 경우에는
                targetPoint += swap.ToPoint();
                targetPoint += direction.Opposite().ToPoint();

                if(!rect.Contains((Vector2)targetPoint)) {
                    inlineViews = null;
                    break;
                }

                inlineViews = _currentViews.Where(v => v != selectedView && (v.position * mask == targetPoint * mask));
                var first = inlineViews.OrderByDescending(view => (view.position.x + view.position.z) * (mask.x + mask.z)).LastOrDefault();
                targetPoint = first.position;

            } else targetPoint += direction.ToPoint();
        }

        //

        if(IsPositive) selectedView = inlineViews?.FirstOrDefault(view => view.position == targetPoint) ?? selectedView;
        else selectedView = inlineViews?.LastOrDefault(view => view.position == targetPoint);
        
        return selectedView == null;
    }

    private void OnDrawGizmos() {
        if(selectedView == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(selectedView.transform.position, 1f);
    }

    public IAssignableView GetBoundaryViewInDirection(Direction direction) {
        if(direction == Direction.None) return null;

        return _currentViews.OrderByDescending(view =>
            Vector3.Dot(direction.Opposite().ToPoint(), Camera.main.WorldToViewportPoint(view.transform.position)))
            .FirstOrDefault();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _view.OnViewCreated?.AddListener(Initialize);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _view.OnViewCreated?.RemoveAllListeners();
    }
}

public partial class ViewInputSystem : Selectable
{
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

        if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
            return dx > 0 ? Direction.Right : Direction.Left;
        } else {
            return dy > 0 ? Direction.Up : Direction.Down;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        selectedView = null;
        base.OnDeselect(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        Direction direction = eventData.moveVector.ToDirection();
        if(!OnMoveInput(direction)) return;

        base.OnMove(eventData);
    }

    protected override void Reset() {
        base.Reset();

        transition = Transition.None;
        Navigation navigation = new()
        {
            mode = Navigation.Mode.Explicit | Navigation.Mode.Automatic
        };
        
        this.navigation = navigation;
    }
}
