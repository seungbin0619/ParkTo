using UnityEngine;

public class TriggerView : MonoBehaviour
{
    private  TriggerType _type;
    private uint _count;
    private bool _isInfinite => _count == uint.MaxValue;

    public void Initialize(TriggerType type, uint count) {
        _type = type;
        _count = count;

        ApplyVisual();
    }

    private void ApplyVisual() {
        // ...
    }
}