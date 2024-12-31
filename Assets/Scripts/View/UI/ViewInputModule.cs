using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewInputModule : Selectable
{
    [SerializeField] private LevelView _view;
    [SerializeField] private LevelGenerator _generator;

    private GroundView selectedView = null;

    protected override void Start() {
        base.Start();
        
        Select();
    }

    // Test
    private void OnDrawGizmos() {
        if(!selectedView) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(selectedView.transform.position, 1f);
    }

    public override void OnMove(AxisEventData eventData)
    {
        Direction direction = eventData.moveVector.ToDirection();

        if(TrySelectNextView(direction)) {
            return;
        }

        base.OnMove(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        TrySelectNextView(GetLastInput());

        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        selectedView = null;
        base.OnDeselect(eventData);
    }

    public GroundView GetBoundaryGroundInDirection(Direction direction) {
        if(direction == Direction.None) return null;
        if(!_view) return null;

        return _view.GroundViews.Values.OrderByDescending(view =>
            Vector3.Dot(direction.ToPoint(), Camera.main.WorldToViewportPoint(view.transform.position)))
            .FirstOrDefault();
    }

    public bool TrySelectNextView(Direction direction) {
        if(selectedView == null) {
            SelectView(GetBoundaryGroundInDirection(direction.Opposite()));
            return true;
        }

        Point position = selectedView.Ground.Position;
        do {
            position = position.Next(direction);

            if(_generator.Grid.IsOutOfBounds(position)) {
                return false;
            }
        } while(!_view.GroundViews.ContainsKey(position));
        
        SelectView(_view.GroundViews[position]);
        return true;
    }

    public void SelectView(GroundView view) {
        if(!view) return;

        selectedView = view;
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

    protected override void OnEnable()
    {
        base.OnEnable();

        //_view.OnViewCreated?.AddListener(Initialize);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        //_view.OnViewCreated?.RemoveAllListeners();
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
