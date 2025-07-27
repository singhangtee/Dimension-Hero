using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class FinalYesPortal : MonoBehaviour
{
    public RawImage videoScreen;      // Assign your RawImage here
    public CanvasGroup canvasGroup;   // Assign your CanvasGroup here
    public VideoPlayer videoPlayer;   // Assign your VideoPlayer here
    public float fadeDuration = 1f;

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;
            StartCoroutine(PlayVideosWithLoop());
        }
    }

    private IEnumerator PlayVideosWithLoop()
    {
        // Fade in
        yield return StartCoroutine(FadeCanvas(0, 1, fadeDuration));

        // Play first video once (Yes Ending.mp4)
        string firstVideo = System.IO.Path.Combine(Application.streamingAssetsPath, "Yes Ending.mp4");
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = firstVideo;
        videoPlayer.isLooping = false;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();

        while (videoPlayer.isPlaying)
            yield return null;

        // Fade out and fade in for second video
        yield return StartCoroutine(FadeCanvas(1, 0, fadeDuration));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FadeCanvas(0, 1, fadeDuration));

        // Play second video looped forever (Thank you.mp4)
        string secondVideo = System.IO.Path.Combine(Application.streamingAssetsPath, "Thank You.mp4");
        videoPlayer.url = secondVideo;
        videoPlayer.isLooping = true;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();

        // Keep canvas visible forever with looping video
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
