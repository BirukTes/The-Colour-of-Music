using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using System.IO;

public class ButtonController : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite playSolid;
    public Sprite pauseSolid;
    public Sprite compressSolid;
    public Sprite expandSolid;


    private AudioSource audioSource;
    private Image playPauseBtnImage;
    private bool playPauseBtnImage_SetPlay;
    private bool playPauseBtnImage_SetPause;
    private Image fullscreeBtnImage;
    private bool fullscreeBtnImage_SetCompress;
    private bool fullscreeBtnImage_SetExpand;


    public GameObject AppController; 
    public Toggle ToggleVideo;
    public Toggle TogglePicture; 


    [Header("Recorder Variables")]
    public Camera mainCamera;
    public RenderTexture renderTexture;

    [Header("To Enable/Disable (on Recorder)")]
    public GameObject secondaryCameraGO;
    public GameObject controlPanelGO;
    public GameObject secondaryCanvasGO;

    private bool enableDisabled_DuringRecord = false;
    private bool enableDisabled_AfterRecord = false;
    private bool windowMaximised = false;

    // private RecorderController recorderController;
    private int defWidth;
    private int defHeight;
    public void Awake()
    {
        defWidth = Screen.width;
        defHeight = Screen.height;
    }

    void Start()
    {
        // Fetch the AudioSource from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();

        playPauseBtnImage = GameObject.Find("ButtonPlayPause").GetComponent<Image>();
        fullscreeBtnImage = GameObject.Find("ImageFullScreen").GetComponent<Image>();
    }

    void Update()
    {
        // if (recorderController != null)
        // {
        //     enableDisableObjects();
        // }

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

        updateSprites();
    }


    public void ScreenControl()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }

    public void PlayPause()
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
    // public void StartVideoRecording()
    // {
    //     if (renderTexture == null)
    //     {
    //         Debug.LogError($"You must assign a valid renderTexture before entering Play Mode");
    //         return;
    //     }

    //     RecorderOptions.VerboseMode = true;

    //     var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
    //     recorderController = new RecorderController(controllerSettings);

    //     var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
    //     videoRecorder.name = "My Video Recorder";
    //     videoRecorder.Enabled = true;
    //     // videoRecorder.OutputFile = "Recordings/PicTarot_" + currentCardId;
    //     videoRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;

    //     videoRecorder.ImageInputSettings = new RenderTextureInputSettings()
    //     {
    //         OutputWidth = renderTexture.width,
    //         OutputHeight = renderTexture.height,
    //         RenderTexture = renderTexture
    //     };

    //     videoRecorder.AudioInputSettings.PreserveAudio = true;

    //     controllerSettings.AddRecorderSettings(videoRecorder);
    //     controllerSettings.SetRecordModeToFrameInterval(0, 10 * 30); // 2s @ 30 FPS
    //     controllerSettings.FrameRate = 30;
    //     recorderController.PrepareRecording();
    //     recorderController.StartRecording();
    // }

    public void selectAudioFile()
    {
        var bp = new BrowserProperties();
        bp.title = "Select Audio File";
        bp.filter = "Supported Audio Files (*.wav;*.ogg;*.mp3)|*.wav;*.ogg;*.mp3";
        bp.filterIndex = 2;

        new FileBrowser().OpenFileBrowser(bp, result =>
        {
            // resultText.text = result;
            Main.selectedAudioPath = result;
            Main.selectedAudioPathName = Path.GetFileName(result);

            // AudioProcessor.setAudioData();
            Main.audioFileChanged = true;

            Debug.Log(result);
        });
    }

    public void ChangeToMovie()
    {
        if (ToggleVideo.isOn)
        {
            AppController.GetComponent<AudioSyncColor>().enabled = false;
            AppController.GetComponent<AudioSyncColor>().enabled = false;
            
        }
    }

    public void ChangeToPicture()
    {

    }

    private void updateSprites()
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
                fullscreeBtnImage.sprite = compressSolid;
                fullscreeBtnImage_SetCompress = true;
                fullscreeBtnImage_SetExpand = false;
            }
        }
        else
        {
            if (!fullscreeBtnImage_SetExpand)
            {
                fullscreeBtnImage.sprite = expandSolid;
                fullscreeBtnImage_SetExpand = true;
                fullscreeBtnImage_SetCompress = false;
            }
        }
    }

    // private void enableDisableObjects()
    // {
    //     if (recorderController.IsRecording())
    //     {
    //         if (!enableDisabled_DuringRecord)
    //         {
    //             controlPanelGO.SetActive(false);
    //             mainCamera.targetTexture = renderTexture;

    //             secondaryCanvasGO.SetActive(true);
    //             secondaryCameraGO.SetActive(true);

    //             enableDisabled_DuringRecord = true;
    //             enableDisabled_AfterRecord = false;
    //         }
    //     }
    //     else
    //     {
    //         if (!enableDisabled_AfterRecord)
    //         {
    //             controlPanelGO.SetActive(true);
    //             mainCamera.targetTexture = null;

    //             secondaryCanvasGO.SetActive(false);
    //             secondaryCameraGO.SetActive(false);

    //             enableDisabled_AfterRecord = true;
    //             enableDisabled_DuringRecord = false;
    //         }
    //     }
    // }
}
