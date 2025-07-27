using UnityEngine;
using UnityEngine.Tilemaps;

public class RealityTilemap : MonoBehaviour {
    public RealityState objectReality;
    private TilemapRenderer _tilemapRenderer;
    private TilemapCollider2D _tilemapCollider;

    private void Start()
    {
        _tilemapCollider = GetComponent<TilemapCollider2D>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
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
    
        if (_tilemapRenderer) _tilemapRenderer.enabled = isActive;
        if (_tilemapCollider) _tilemapCollider.enabled = isActive;
    }
}