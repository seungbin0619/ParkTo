using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Triggers {
    public event Action<TriggerType> OnTriggerUsed = delegate {};
    public event Action<TriggerType> OnTriggerCancelled = delegate {};

    public const uint Infinity = uint.MaxValue;
    private readonly Dictionary<TriggerType, uint> _triggers;

    public IEnumerable<TriggerType> Types => _triggers.Keys;

    public Triggers(IEnumerable<TriggerSerializer> triggers) {
        _triggers = new();
        _triggers.AddRange(triggers.Select(trigger => 
            new KeyValuePair<TriggerType, uint>(trigger.type, 
                trigger.isInfinite 
                    ? Infinity 
                    : trigger.count)));
    }

    public uint this[TriggerType type] => Count(type);

    public uint Count(TriggerType type) {
        return _triggers.ContainsKey(type) ? _triggers[type] : 0;
    }

    public bool IsEnabled(TriggerType type) {
        return _triggers.ContainsKey(type) && _triggers[type] > 0;
    }

    public void Use(TriggerType type) {
        if(!IsEnabled(type)) return;
        if(_triggers[type] == Infinity) return;

        _triggers[type]--;
        OnTriggerUsed?.Invoke(type);
    }

    public void Use(Trigger trigger) {
        Use(trigger.Type);
    }

    public void Cancel(TriggerType type) {
        if(!_triggers.ContainsKey(type)) return;
        if(_triggers[type] == Infinity) return;

        _triggers[type]++;
        OnTriggerCancelled?.Invoke(type);
    }

    public void Cancel(Trigger trigger) {
        Cancel(trigger.Type);
    }
}