using UnityEngine;

public class RealityObject : MonoBehaviour {
    public RealityState objectReality;
    private Renderer _renderer;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
    }

    void OnEnable() {
        RealityManager.OnRealitySwitched += UpdateVisibility;
        UpdateVisibility();
    }

    void OnDisable()
    {
        RealityManager.OnRealitySwitched -= UpdateVisibility;
    }

    void UpdateVisibility() {
        bool isActive = RealityManager.CurrentReality == objectReality;
    
        if (_renderer) _renderer.enabled = isActive;
        if (_collider) _collider.enabled = isActive;
    }
}
