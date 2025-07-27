using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Video;
using UnityEngine.Networking;

public class WebGlPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    // public string yesEnding = "https://github.com/singhangtee/Dimension-Hero/blob/main/Assets/Video/yes_ending.webm";
    // public string thankYou = "https://github.com/singhangtee/Dimension-Hero/blob/main/Assets/Video/THANK_YOU.webm";
    
    
    void Start()
    {
        StartCoroutine(LoadVideo());
    }

    IEnumerator LoadVideo()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "yes_ending.webm");

        // WebGL returns a URL, not a file path
        if (videoPath.Contains("://") || videoPath.Contains(":///"))
        {
            videoPlayer.url = videoPath;
            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
            rawImage.texture = videoPlayer.targetTexture;
            videoPlayer.Play();
        }

        yield return null;
    }
}
