using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RockVR.Video;

public class ScreenRecorder : MonoBehaviour
{
    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    public static List<Texture2D> screenShotList = new List<Texture2D>();

    // commands
    public static bool finishedScreenshot;
    public static bool finishedVideoCapture;
    public static bool takeVideo;
    public static bool isVideoCaptureInProcess;

    void Update()
    {
        TakeScreenShot();

        if (takeVideo) // According to the user's needs
            TakeVideo();
    }

    public void TakeScreenShot()
    {
        if (Main.audioSource.isPlaying && !finishedScreenshot)
        {
            StartCoroutine("TakeScreenShotInCoroutine");
        }
        else
        {
            if (!Main.audioSource.isPlaying && (Main.audioFinishedPlaying && !finishedScreenshot))
            {
                StopCoroutine("TakeScreenShotInCoroutine");
                finishedScreenshot = true;

                Debug.Log("list : " + screenShotList.Count);
            }
        }
    }


    public void TakeVideo()
    {
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START && Main.audioSource.isPlaying) // if ("Not Started Capture") => Start
        {
            VideoCaptureCtrl.instance.StartCapture();
            Debug.Log("Started");
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED && !Main.audioSource.isPlaying && !Main.audioFinishedPlaying) // "Pause Capture"
        {
            VideoCaptureCtrl.instance.ToggleCapture();
            Debug.Log("Pause");
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED && Main.audioSource.isPlaying) // "Continue Capture"
        {
            VideoCaptureCtrl.instance.ToggleCapture();
            Debug.Log("Continue");
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED && Main.audioFinishedPlaying) // "Stop Capture"
        {
            VideoCaptureCtrl.instance.StopCapture(); // Will process the video
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED && !isVideoCaptureInProcess)
        {
            // "Processing" => nothing to do
            isVideoCaptureInProcess = true;
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH && !finishedVideoCapture)
        {
            // "Finished"
            finishedVideoCapture = true;
            takeVideo = false;
            isVideoCaptureInProcess = false;
        }
    }

    private IEnumerator TakeScreenShotInCoroutine()
    {
        yield return new WaitForEndOfFrame();

        screenShotList.Add(ScreenCapture.CaptureScreenshotAsTexture());
        screenShotList[screenShotList.Count - 1].Compress(false); // Reduce memory footprint 

        yield break;
    }


    //// RESET VALUES    
    public static void ResetValues()
    {
        finishedScreenshot = false;
        finishedVideoCapture = false;
        takeVideo = false;
        isVideoCaptureInProcess = false;

        if (screenShotList.Count > 0)
        {
            screenShotList.ForEach(texture =>
            {
                Destroy(texture);
            }); // Reset the list
            screenShotList = new List<Texture2D>();
        }
    }
}