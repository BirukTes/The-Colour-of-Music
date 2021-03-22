using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RockVR.Video;

public class ScreenRecorder : MonoBehaviour
{
    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    public static List<Texture2D> screenShotList = new List<Texture2D>();

    // commands
    private static bool finishedScreenshot = false;
    private static bool finishedVideoCapture = false;
    public static bool finishedRecording = false;
    public static bool isVideoCapturing = false;
    public static bool isVideoCaptureInProcess = false;

    void Update()
    {
        if (finishedScreenshot) // && finishedVideoCapture
        {
            finishedRecording = true;
        }
        else
        {
            // TakeScreenShot();
            TakeVideo();
        }
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
            Debug.Log("Stop");
            VideoCaptureCtrl.instance.StopCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED && !isVideoCaptureInProcess)
        {
            // "Processing" => nothing to do
            isVideoCaptureInProcess = true;
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH && !finishedVideoCapture)
        {
            // if ("Finished")
            finishedVideoCapture = true;
            isVideoCaptureInProcess = false;
        }
    }

    private IEnumerator TakeScreenShotInCoroutine()
    {
        yield return new WaitForEndOfFrame();

        screenShotList.Add(ScreenCapture.CaptureScreenshotAsTexture());

        yield break;
    }


    //// RESET VALUES    
    public static void ResetValues()
    {
        finishedScreenshot = false;
        finishedVideoCapture = false;
        finishedRecording = false;
        screenShotList = new List<Texture2D>(); // Reset values
    }
}