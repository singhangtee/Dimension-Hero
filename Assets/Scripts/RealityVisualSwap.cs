using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RealityVisualSwap : MonoBehaviour {
    public Sprite spriteRealityNormal;
    public Sprite spriteRealityHero;

    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        RealityManager.OnRealitySwitched += UpdateVisual;
        UpdateVisual();
    }

    void OnDisable() {
        RealityManager.OnRealitySwitched -= UpdateVisual;
    }

    void UpdateVisual() {
        spriteRenderer.sprite = RealityManager.CurrentReality == RealityState.RealityNormal
            ? spriteRealityNormal
            : spriteRealityHero;
    }
}
