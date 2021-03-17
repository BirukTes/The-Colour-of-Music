using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    public static string selectedAudioPath;
    public static string selectedAudioPathName;

    public static bool audioFileChanged = false;
    public static bool audioClipChanged = false;


    //Make sure your GameObject has an AudioSource component first
    private static AudioSource audioSource;
    private string clipLength;

    public TextMeshProUGUI textFileName;
    public TextMeshProUGUI textRunningTime;
    public RectTransform rectT; // Assign the UI element which you wanna capture
    private int width; // width of the object to capture
    private int height; // height of the object to capture

    void Awake()
    {
        selectedAudioPath = Application.dataPath + "/Lib/sonic-annotator-1.6-win64/audio.ogg";
        selectedAudioPathName = Path.GetFileName(selectedAudioPath);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the AudioSource from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();

        updateMediaInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("herea");
            // ScreenCapture.CaptureScreenshot("FullPageScreenShot.png");
            // StartCoroutine(takeScreenShot()); // screenshot of a particular UI Element.
            this.GetComponent<AddImagesGrid>().enabled = true;
        }

        if (audioFileChanged)
        {
            StartCoroutine(changeClip());
        }
        else if (audioClipChanged)
        {
            updateMediaInfo();
            audioClipChanged = false;
        }

        updateRunningTime();
    }

    ////////////////////////  PUBLIC ///////////////////


    ////////////////////////  PRIVATE ///////////////////
    private void updateRunningTime()
    {
        if (audioSource.isPlaying)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(audioSource.time);

            textRunningTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds) + " / " + clipLength;
        }

    }

    private IEnumerator changeClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + selectedAudioPath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                if (myClip == null)
                {
                    Debug.Log("Clip null");
                }
                else
                {
                    audioSource.Stop();
                    audioSource.clip = myClip;
                    audioSource.Play();
                    audioFileChanged = false;
                    audioClipChanged = true;
                }
            }
        }
    }

    private void updateMediaInfo()
    {
        TimeSpan clipLengthTimeSpan = TimeSpan.FromSeconds(audioSource.clip.length);
        clipLength = string.Format("{0:D2}:{1:D2}", clipLengthTimeSpan.Minutes, clipLengthTimeSpan.Seconds);

        // Set fileName    
        textFileName.text = selectedAudioPathName;
        // So first lets check if we already have a tooltip
        var tooltip = textFileName.GetComponent<SimpleTooltip>();
        if (tooltip)
        {
            tooltip.infoLeft = selectedAudioPathName;
        }
    }
}
