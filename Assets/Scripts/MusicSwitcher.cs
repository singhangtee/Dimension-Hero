using System.Collections;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [Header("Music Clips")]
    public AudioClip normalRealityMusic;
    public AudioClip heroRealityMusic;
    
    [Header("Settings")]
    public bool loopMusic = true;
    public bool useFadeTransition = true;
    public float fadeSpeed = 2f;
    
    private AudioSource _audioSource;
    private bool _isFading = false;
    
    private void Start()
    {
        // Get or create AudioSource
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
            _audioSource = gameObject.AddComponent<AudioSource>();
            
        // Configure AudioSource
        _audioSource.loop = loopMusic;
        _audioSource.playOnAwake = false;
        
        // Subscribe to reality changes
        RealityManager.OnRealitySwitched += OnRealityChanged;
        
        // Start with the correct music for current reality
        PlayMusicForCurrentReality();
    }
    
    private void OnRealityChanged()
    {
        if (useFadeTransition && _audioSource.isPlaying)
        {
            // Fade out current music, then fade in new music
            StartCoroutine(FadeToNewMusic());
        }
        else
        {
            // Instantly switch music
            PlayMusicForCurrentReality();
        }
    }
    
    private void PlayMusicForCurrentReality()
    {
        AudioClip clipToPlay = RealityManager.CurrentReality == RealityState.RealityHero 
            ? heroRealityMusic 
            : normalRealityMusic;
            
        if (clipToPlay)
        {
            _audioSource.clip = clipToPlay;
            _audioSource.volume = 1f;
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
            
            #if DEVELOPMENT_BUILD   
                Debug.LogWarning("No music clip assigned for: " + RealityManager.CurrentReality);
            #endif
        }
    }
    
    private IEnumerator FadeToNewMusic()
    {
        if (_isFading) yield break; // Prevent multiple fades at once
        _isFading = true;
        
        // Fade out current music
        float startVolume = _audioSource.volume;
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        
        // Switch to new music
        AudioClip newClip = RealityManager.CurrentReality == RealityState.RealityHero 
            ? heroRealityMusic 
            : normalRealityMusic;
            
        if (newClip)
        {
            _audioSource.clip = newClip;
            _audioSource.Play();
            
            // Fade in new music
            while (_audioSource.volume < 1f)
            {
                _audioSource.volume += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            _audioSource.volume = 1f;
        }
        else
        {
            _audioSource.Stop();
            
            #if DEVELOPMENT_BUILD   
                Debug.LogWarning("No music clip assigned for: " + RealityManager.CurrentReality);
            #endif  
        }
        
        _isFading = false;
    }
    
    private void OnDestroy()
    {
        RealityManager.OnRealitySwitched -= OnRealityChanged;
    }
}