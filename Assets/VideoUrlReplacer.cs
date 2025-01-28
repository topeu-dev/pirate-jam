using UnityEngine;
using UnityEngine.Video;

public class VideoUrlReplacer : MonoBehaviour
{
    public string fileName;
    void Start()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = Application.streamingAssetsPath + "/" + fileName;
    }
}
