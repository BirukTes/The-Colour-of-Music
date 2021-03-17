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

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    public static List<Texture2D> screenShotList = new List<Texture2D>();
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;


    public int fps = 10;
    private float videoTime = 0;

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    void Update()
    {
        // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
        captureScreenshot |= Input.GetKeyDown("k");
        captureVideo = Input.GetKey("v");

        if (captureScreenshot || captureVideo)
        {
            captureScreenshot = false;

            videoTime += Time.deltaTime;

            if (videoTime >= (1f / (float)fps))
            {
                Debug.LogFormat(videoTime.ToString());
                videoTime = 0;

                // hide optional game object if set
                if (hideGameObject != null) hideGameObject.SetActive(false);

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


                // unhide optional game object if set
                if (hideGameObject != null) hideGameObject.SetActive(true);

                // cleanup if needed
                if (optimizeForManyScreenshots == false)
                {
                    Destroy(renderTexture);
                    renderTexture = null;
                    screenShot = null;
                }
                Debug.Log("list : " + screenShotList.Count);

            }
        }
    }
}