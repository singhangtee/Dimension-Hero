using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBg : MonoBehaviour
{
    private GameObject _bg1;
    private GameObject _bg2;

    // Start is called before the first frame update
    void Awake()
    {
        _bg1 = GameObject.FindGameObjectWithTag("BG1");
        _bg2 = GameObject.FindGameObjectWithTag("BG2");
        _bg2.SetActive(false);
    }

    void OnEnable() {
        RealityManager.OnRealitySwitched +=  OnRealityChanged;
        OnRealityChanged();
    }

    void OnDisable()
    {
        RealityManager.OnRealitySwitched -=  OnRealityChanged;
    }
    
    
    private void OnRealityChanged()
    {
        if (RealityManager.CurrentReality == RealityState.RealityNormal)
        {
            // Fade out current music, then fade in new music
            _bg1.SetActive(true);
            _bg2.SetActive(false);
        }
        else
        {
            _bg1.SetActive(false);
            _bg2.SetActive(true);
        }
    }
}
