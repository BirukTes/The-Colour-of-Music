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
    public float audioRunningTime = 30f;

    public static string selectedAudioPath;
    public static string audioPathInUseForProcessing;
    public static string defaultAudioPath;
    public static string selectedAudioPathName;
    public static string defaultAudioPathName;
    public static bool audioFileChanged;
    public static bool audioClipChanged;
    public static bool resetAudioToDefault;
    public static bool audioFinishedPlaying;
    public static bool addImagesToGridisProcessing;
    public static bool firstTimePlay = true;
    public static int shouldDisableBtns = -1; // -1 == not set, 0=false, 1=true


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
    public Toggle videoToggle;

    private int width; // width of the object to capture
    private int height; // height of the object to capture
    private string clipLength;
    private bool windowMaximised;
    private bool disabledBtnsOn;
    private bool waitVideoModeToBeOn;
    private bool pictureModeWasOnSoSwitchBack;
    private Coroutine co_HideCursor;

    void Awake()
    {
        defaultAudioPath = Application.streamingAssetsPath + "/TCM/Audio/Ludwig van Beethoven - Für Elise Bagatelle No. 25 in A Minor, WoO 59 (Lang Lang).ogg";
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
        if (audioSource.isPlaying && audioSource.time >= (audioRunningTime - 0.5)) // minus 0.5 to 59.5, to prevent audio finishing before
        {
            audioSource.Stop();
            audioFinishedPlaying = true;
            shouldDisableBtns = 0;

            if (pictureModeWasOnSoSwitchBack)
            {
                videoToggle.isOn = false;
                pictureToggle.isOn = true;

                pictureModeWasOnSoSwitchBack = false;
            }
        }
        else //if (audioSource.isPlaying)
        {
            audioFinishedPlaying = false;
        }


        //When the user hits the spacebar, pause/play, but not while adding images and only works while playing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                StartCoroutine(playAudio());
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
            ScreenRecorder.ResetValues();
            AddImagesToGrid.ResetValues();
            StartCoroutine(AudioProcessor.SetAudioData(false));

            audioClipChanged = false;


            // Change of file during picture mode
            if (pictureToggle.isOn)
            {
                waitVideoModeToBeOn = true;
                pictureModeWasOnSoSwitchBack = true;
                pictureToggle.isOn = false;
                videoToggle.isOn = true;
            }
            else
            {
                StartCoroutine(playAudio());
            }
        }


        if (waitVideoModeToBeOn)
        {
            if (videoToggle.isOn && !audioSource.isPlaying)
            {
                StartCoroutine(playAudio());
            }

            waitVideoModeToBeOn = false;
        }


        UpdateFullscreen();
        UpdateSprites();
        UpdateRunningTime();
        HideWhilePlaying();

        UpdateDisableBtns();

        IEnumerator playAudio()
        {
            // Wait to allow anything else finish e.g. introPanel, black rest colour, intensity
            yield return new WaitForSeconds(0.2f);
            if (firstTimePlay)
            {
                GetComponent<DestoryIntroPanel>().enabled = true;
                firstTimePlay = false;
            }
            audioSource.Play();

            yield break;
        }
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
        shouldDisableBtns = 1;
        if (resetAudioToDefault)
        {
            var nameWithoutExt = defaultAudioPathName.Replace(Path.GetExtension(defaultAudioPathName), "");
            //Load the sound
            changeToClip(Resources.Load<AudioClip>(nameWithoutExt));
            resetAudioToDefault = false;
            ProgressBar.IncrementProgressBar(0.7f);
        }
        else
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
                        changeToClip(myClip);

                        if (audioType == AudioType.MPEG)
                        {
                            audioPathInUseForProcessing = StringUtil.Replace(selectedAudioPath, ".mp3", ".wav", StringComparison.OrdinalIgnoreCase);
                            SavWav.Save(audioPathInUseForProcessing, myClip, false);
                        }
                        ProgressBar.IncrementProgressBar(0.7f);
                    }
                }
            }
        }

        void changeToClip(AudioClip changeToClip)
        {
            audioSource.Stop();
            audioSource.clip = changeToClip;
            ProgressBar.IncrementProgressBar(0.5f);

            audioFileChanged = false;
            audioClipChanged = true;
        }

        yield break;
    }

    private void UpdateMediaInfo()
    {
        TimeSpan clipLengthTimeSpan = TimeSpan.FromSeconds(audioRunningTime);
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


        IEnumerator HideCursor()
        {
            yield return new WaitForSeconds(3);
            Cursor.visible = false;
        }
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
