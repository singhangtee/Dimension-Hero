using UnityEngine;

public class RealityObject : MonoBehaviour {
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
        Renderer renderer = GetComponent<Renderer>();
        Collider2D collider = GetComponent<Collider2D>();
    
        bool isActive = RealityManager.CurrentReality == objectReality;
    
        if (renderer) renderer.enabled = isActive;
        if (collider) collider.enabled = isActive;
    }
}
