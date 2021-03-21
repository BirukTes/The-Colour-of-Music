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

    void Update()
    {
        if (finishedScreenshot) // && finishedVideoCapture
        {
            finishedRecording = true;
        }
        else
        {
            TakeScreenShot();
            // TakeVideo();            
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
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED && !Main.audioSource.isPlaying) // "Pause Capture"
        {
            VideoCaptureCtrl.instance.ToggleCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED && Main.audioSource.isPlaying) // "Continue Capture"
        {
            VideoCaptureCtrl.instance.ToggleCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED && Main.audioFinishedPlaying) // "Stop Capture"
        {
            VideoCaptureCtrl.instance.StopCapture();
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
        {
            // "Processing" => nothing to do
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH)
        {
            // if ("Finished")
            finishedVideoCapture = true;
        }
    }

    private IEnumerator TakeScreenShotInCoroutine()
    {
        hideGameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        screenShotList.Add(ScreenCapture.CaptureScreenshotAsTexture());

        hideGameObject.SetActive(true);
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