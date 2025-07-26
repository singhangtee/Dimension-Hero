using UnityEngine;
using UnityEngine.Tilemaps;

public class RealityTilemap : MonoBehaviour {
    public RealityState objectReality;

    void OnEnable() {
        RealityManager.OnRealitySwitched += UpdateVisibility;
        UpdateVisibility();
    }

    void OnDisable()
    {
        RealityManager.OnRealitySwitched -= UpdateVisibility;
    }

    void UpdateVisibility() {
        TilemapRenderer tilemapRenderer = GetComponent<TilemapRenderer>();
        TilemapCollider2D tilemapCollider = GetComponent<TilemapCollider2D>();
    
        bool isActive = RealityManager.CurrentReality == objectReality;
    
        if (tilemapRenderer) tilemapRenderer.enabled = isActive;
        if (tilemapCollider) tilemapCollider.enabled = isActive;
    }
}