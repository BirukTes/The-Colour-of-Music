using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //Static
    public static AudioSource audioSource;

    public static string selectedAudioPath;
    public static string audioPathInUseForProcessing;
    public static string defaultAudioPath;
    public static string selectedAudioPathName;
    public static string defaultAudioPathName;
    public static bool audioFileChanged;
    public static bool audioClipChanged;
    public static bool audioFinishedPlaying;
    public static bool addImagesToGridisProcessing;
    public static int  shouldDisableBtns = -1; // -1 == not set, 0=false, 1=true


    [Header("Sprites")]
    public Sprite playSolid;
    public Sprite pauseSolid;
    public Sprite compressSolid;
    public Sprite expandSolid;

    [Space]
    public Image playPauseBtnImage;
    private bool playPauseBtnImage_SetPlay;
    private bool playPauseBtnImage_SetPause;
    public Image fullscreenBtnImage;
    private bool fullscreeBtnImage_SetCompress;
    private bool fullscreeBtnImage_SetExpand;


    [Space]
    public TextMeshProUGUI textFileName;
    public TextMeshProUGUI textRunningTime;

    [Space]
    public GameObject controlsGO;
    public Button saveButton;
    public Toggle pictureToggle;

    private int width; // width of the object to capture
    private int height; // height of the object to capture
    private string clipLength;
    private bool windowMaximised;
    private bool disabledBtnsOn;
    private Coroutine co_HideCursor;

    void Awake()
    {
        defaultAudioPath = Application.dataPath + "/Audio/Lang Lang – Beethoven Für Elise Bagatelle No. 25 in A Minor, WoO 59.ogg";
        defaultAudioPathName = Path.GetFileName(defaultAudioPath);
        selectedAudioPath = defaultAudioPath;
        selectedAudioPathName = defaultAudioPathName;
        audioPathInUseForProcessing = defaultAudioPath;

        // Fetch the AudioSource from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMediaInfo();
    }

    // Update is called once per frame
    void Update()
    {
        //When the user hits the spacebar, pause/play, but not while adding images and only works while playing
        if (Input.GetKeyDown(KeyCode.Space) && !addImagesToGridisProcessing)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }

        if (audioFileChanged)
        {
            StartCoroutine(ChangeClip());
        }
        else if (audioClipChanged)
        {
            UpdateMediaInfo();

            AudioProcessor.ResetValues();
            AudioProcessor.SetAudioData(false);
            ScreenRecorder.ResetValues();
            AddImagesToGrid.ResetValues();

            ProgressBar.IncrementProgressBar(1f);
            audioSource.Play();
            shouldDisableBtns = 1;
            audioClipChanged = false;
            ProgressBar.DisableProgressBarGO();
        }

        if (audioSource.isPlaying && audioSource.time >= 30)
        {
            audioSource.Stop();
            audioFinishedPlaying = true;
            shouldDisableBtns = 0;
        }
        else
        {
            audioFinishedPlaying = false;
        }

        UpdateFullscreen();
        UpdateSprites();
        UpdateRunningTime();
        HideWhilePlaying();

        UpdateDisableBtns();
    }

    ////////////////////////  PUBLIC ///////////////////


    #region ////////////////////////  PRIVATE ///////////////////
    private void UpdateFullscreen()
    {
        if (Screen.fullScreen)
        {
            windowMaximised = false;
        }
        else
        {
            if (!windowMaximised)
            {
                MaximizeStandaloneWindow.MaximizeWindow();

                windowMaximised = true;
            }
        }
    }

    private void UpdateRunningTime()
    {
        if ((audioSource.isPlaying && controlsGO.activeSelf) || audioFinishedPlaying)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(audioSource.time);

            textRunningTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds) + " / " + clipLength;
        }
    }

    private IEnumerator ChangeClip()
    {
        AudioType audioType = AudioType.MPEG;
        var extension = Path.GetExtension(selectedAudioPath);
        if (extension == ".wav")
        {
            audioType = AudioType.WAV;
        }
        else if (extension == ".ogg")
        {
            audioType = AudioType.OGGVORBIS;
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + selectedAudioPath, audioType))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                ProgressBar.IncrementProgressBar(0.3f);

                if (myClip == null)
                {
                    Debug.Log("Clip null");
                }
                else
                {
                    audioSource.Stop();
                    audioSource.clip = myClip;
                    ProgressBar.IncrementProgressBar(0.5f);

                    audioFileChanged = false;
                    audioClipChanged = true;

                    if (audioType == AudioType.MPEG)
                    {
                        audioPathInUseForProcessing = StringUtil.Replace(selectedAudioPath, ".mp3", ".wav", StringComparison.OrdinalIgnoreCase);
                        SavWav.Save(audioPathInUseForProcessing, myClip, false);
                    }
                    ProgressBar.IncrementProgressBar(0.7f);
                }
            }
        }

        yield break;
    }

    private void UpdateMediaInfo()
    {
        TimeSpan clipLengthTimeSpan = TimeSpan.FromSeconds(30);
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


    private void UpdateSprites()
    {
        if (audioSource.isPlaying)
        {
            if (!playPauseBtnImage_SetPause)
            {
                playPauseBtnImage.sprite = pauseSolid;
                playPauseBtnImage_SetPause = true;
                playPauseBtnImage_SetPlay = false;
            }
        }
        else
        {
            if (!playPauseBtnImage_SetPlay)
            {
                playPauseBtnImage.sprite = playSolid;
                playPauseBtnImage_SetPause = false;
                playPauseBtnImage_SetPlay = true;
            }
        }

        if (Screen.fullScreen)
        {
            if (!fullscreeBtnImage_SetCompress)
            {
                fullscreenBtnImage.sprite = compressSolid;
                fullscreeBtnImage_SetCompress = true;
                fullscreeBtnImage_SetExpand = false;
            }
        }
        else
        {
            if (!fullscreeBtnImage_SetExpand)
            {
                fullscreenBtnImage.sprite = expandSolid;
                fullscreeBtnImage_SetExpand = true;
                fullscreeBtnImage_SetCompress = false;
            }
        }
    }

    private void HideWhilePlaying()
    {
        if (audioSource.isPlaying)
        {
            if (Input.GetAxis("Mouse X") == 0 && (Input.GetAxis("Mouse Y") == 0))
            {
                if (co_HideCursor == null)
                {
                    co_HideCursor = StartCoroutine(HideCursor());
                }
            }
            else
            {
                if (co_HideCursor != null)
                {
                    StopCoroutine(co_HideCursor);
                    co_HideCursor = null;
                    Cursor.visible = true;
                }
            }

            controlsGO.SetActive(false);
        }
        else // Pause/Stop
        {
            if (!Cursor.visible)
                Cursor.visible = true;

            controlsGO.SetActive(true);
        }
    }
    private IEnumerator HideCursor()
    {
        yield return new WaitForSeconds(3);
        Cursor.visible = false;
    }


    private void UpdateDisableBtns()
    {
        if (shouldDisableBtns == 1)
        {
            if (disabledBtnsOn)
            {
                saveButton.interactable = false;
                pictureToggle.interactable = false;

                disabledBtnsOn = false;
            }
        }
        else if (shouldDisableBtns == 0)
        {
            if (!disabledBtnsOn)
            {
                saveButton.interactable = true;
                pictureToggle.interactable = true;

                disabledBtnsOn = true;
            }
        }
    }

    #endregion 
}
