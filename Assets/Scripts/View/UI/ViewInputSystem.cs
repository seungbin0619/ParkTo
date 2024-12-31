using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class ViewInputSystem {
    [SerializeField] 
    private LevelView _view;
    
    private List<IAssignableView> _currentViews;
    private IAssignableView selectedView = null;
    
    private void Initialize() {
        _currentViews = new();
        
        _currentViews.AddRange(_view.GroundViews.Values);
        _currentViews.Concat(_view.CarViews);
    }

    // return cancel flag
    private bool OnMoveInput(Direction direction) {
        if(direction == Direction.None) return true;

        if(selectedView == null) {
            selectedView = GetBoundaryViewInDirection(direction);
            return true;
        }
        
        // view의 rotate를 반영한 direction으로 변환환
        direction = selectedView.transform.parent.parent.TransformDirection(direction.ToPoint()).ToDirection();
        Debug.Log(direction);

        return true;
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
        base.OnDeselect(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        Direction direction = eventData.moveVector.ToDirection();
        if(OnMoveInput(direction)) return;

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
