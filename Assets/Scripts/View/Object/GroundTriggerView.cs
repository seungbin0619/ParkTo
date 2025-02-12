using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GroundTriggerView : MonoBehaviour
{
    [SerializeField]
    SerializedDictionary<TriggerType, Material> _triggerMaterials;
    Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetTrigger(TriggerType type) {
        if(!_triggerMaterials.ContainsKey(type)) {
            _renderer.enabled = false;
            return;
        }

        _renderer.enabled = true;
        _renderer.material = _triggerMaterials[type];
    }
}
