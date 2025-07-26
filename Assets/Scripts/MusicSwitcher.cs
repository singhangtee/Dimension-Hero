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
    
    private AudioSource audioSource;
    private bool isFading = false;
    
    private void Start()
    {
        // Get or create AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        // Configure AudioSource
        audioSource.loop = loopMusic;
        audioSource.playOnAwake = false;
        
        // Subscribe to reality changes
        RealityManager.OnRealitySwitched += OnRealityChanged;
        
        // Start with the correct music for current reality
        PlayMusicForCurrentReality();
    }
    
    private void OnRealityChanged()
    {
        if (useFadeTransition && audioSource.isPlaying)
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
            
        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.volume = 1f;
            audioSource.Play();
            Debug.Log("Playing music for: " + RealityManager.CurrentReality);
        }
        else
        {
            audioSource.Stop();
            Debug.LogWarning("No music clip assigned for: " + RealityManager.CurrentReality);
        }
    }
    
    private System.Collections.IEnumerator FadeToNewMusic()
    {
        if (isFading) yield break; // Prevent multiple fades at once
        isFading = true;
        
        // Fade out current music
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        
        // Switch to new music
        AudioClip newClip = RealityManager.CurrentReality == RealityState.RealityHero 
            ? heroRealityMusic 
            : normalRealityMusic;
            
        if (newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
            
            // Fade in new music
            while (audioSource.volume < 1f)
            {
                audioSource.volume += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 1f;
            
            Debug.Log("Faded to music for: " + RealityManager.CurrentReality);
        }
        else
        {
            audioSource.Stop();
            Debug.LogWarning("No music clip assigned for: " + RealityManager.CurrentReality);
        }
        
        isFading = false;
    }
    
    private void OnDestroy()
    {
        RealityManager.OnRealitySwitched -= OnRealityChanged;
    }
}