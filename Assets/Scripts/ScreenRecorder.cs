using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

// Ref: 
//     http://answers.unity.com/answers/1296574/view.html, https://answers.unity.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html

// Screen Recorder will save individual images of active scene in any resolution
public class ScreenRecorder : MonoBehaviour
{
    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;
    private int defaultGoActiveValue = -1; // -1 = not set, 0=false, 1=true


    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    public static List<Texture2D> screenShotList = new List<Texture2D>();

    // commands
    private bool finishedScreenshot = false;
    private bool finishedVideoCapture = false;
    public static bool finishedRecording = false;


    private int fps = 10;
    private float screenShotTime = 0;


    void Update()
    {
        if (Main.audioFinishedPlaying)
        {
            finishedRecording = true;
        }
        else
        {
            if (Main.audioSource.isPlaying && !finishedRecording)
            {
                TakeScreenShot();
            }
        }
    }


    public void TakeScreenShot()
    {
        screenShotTime += Time.deltaTime;

        if (screenShotTime >= (1f / (float)fps))
        {
            Debug.LogFormat(screenShotTime.ToString());
            screenShotTime = 0;

            HideUnhideGO(true);

            // create screenshot objects if needed
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }

            // get main camera and manually render scene into rt
            Camera camera = this.GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
            camera.targetTexture = renderTexture;
            camera.Render();

            // read pixels will read from the currently active render texture so make our offscreen 
            // render texture active and then read the pixels
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            camera.targetTexture = null;
            RenderTexture.active = null;
            screenShotList.Add(screenShot);

            HideUnhideGO(false);

            Debug.Log("list : " + screenShotList.Count);
        }
    }


    public void TakeVideo()
    {

    }

    private void HideUnhideGO(bool value)
    {
        // hide optional game object if set
        if (hideGameObject != null)
        {
            if (defaultGoActiveValue == -1)
                defaultGoActiveValue = hideGameObject.activeSelf ? 1 : 0;

            if (value)
            {
                if (defaultGoActiveValue == 1)
                {
                    hideGameObject.SetActive(false);
                }
            }
            else
            {
                if (defaultGoActiveValue == 1)
                {
                    hideGameObject.SetActive(true);
                }
            }
        }
    }
}