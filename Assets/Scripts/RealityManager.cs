using System;
using UnityEngine;

public enum RealityState {
    RealityNormal,
    RealityHero
}

public class RealityManager : MonoBehaviour {
    public static RealityState CurrentReality { get; private set; } = RealityState.RealityNormal;
    public static event Action OnRealitySwitched;

    public static void SwitchReality() {
        CurrentReality = CurrentReality == RealityState.RealityNormal
            ? RealityState.RealityHero
            : RealityState.RealityNormal;
        
        OnRealitySwitched?.Invoke();
    }
}
