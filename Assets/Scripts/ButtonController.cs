using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using System.IO;
using TMPro;
using System.Collections;
using System;
using RockVR.Video;

public class ButtonController : MonoBehaviour
{
    public Toggle toggleVideo;
    public Toggle togglePicture;


    [Space]
    public TMP_Dropdown fileDropDown;

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
        if (Main.audioSource.isPlaying)
        {
            Main.audioSource.Pause();
        }
        else
        {
            if (Main.firstTimePlay)
            {
                GetComponent<DestoryIntroPanel>().enabled = true;
                Main.firstTimePlay = false;
            }

            Main.audioSource.Play();
        }
    }

    public void SelectAudioFile()
    {
        if (fileDropDown.value == 1)
        {
            var bp = new BrowserProperties();
            bp.title = "Select Audio File";
            bp.filter = "Supported Audio Files (Avoid .MP3 if possible) (*.wav;*.ogg;*.mp3)|*.wav;*.ogg;*.mp3";
            bp.filterIndex = 2;

            new FileBrowser().OpenFileBrowser(bp, result =>
            {
                ProgressBar.EnableProgressBarGO();
                ProgressBar.IncrementProgressBar(0.1f);
                // resultText.text = result;
                Main.selectedAudioPath = result;
                Main.audioPathInUseForProcessing = result;
                Main.selectedAudioPathName = Path.GetFileName(result);

                Main.audioFileChanged = true;

                Debug.Log(result);
            });

            StartCoroutine(ChangeFileDropDownValue(0));
        }
        else if (fileDropDown.value == 2)
        {
            StartCoroutine(ChangeFileDropDownValue(0));


            if (Main.selectedAudioPath != Main.defaultAudioPath)
            {
                Main.selectedAudioPath = Main.defaultAudioPath;
                Main.audioPathInUseForProcessing = Main.defaultAudioPath;
                Main.selectedAudioPathName = Main.defaultAudioPathName;

                Main.audioFileChanged = true;                
                Main.resetAudioToDefault = true;                
            }
        }
    }

    public void ToggleMovieOrPicture()
    {
        if (toggleVideo.isOn)
        {
            GetComponent<AudioProcessor>().enabled = true;
            GetComponent<AudioSyncColor>().enabled = true;
            GetComponent<AddImagesToGrid>().enabled = false;
        }
        else if (togglePicture.isOn)
        {
            GetComponent<AudioProcessor>().enabled = false;
            GetComponent<AudioSyncColor>().enabled = false;
            GetComponent<AddImagesToGrid>().enabled = true;
        }
    }

    public void SaveVisualisation()
    {
        if (toggleVideo.isOn)
        {
            if (ScreenRecorder.finishedVideoCapture)
            {
                if (PathConfig.lastVideoFile != "")
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = PathConfig.lastVideoFile,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            else if (!ScreenRecorder.isVideoCaptureInProcess) // Should not be
            {
                var dialogBoxContainer = GameObject.Find("MainCanvas").transform.Find("DialogBoxContainer").gameObject;
                dialogBoxContainer.SetActive(true);

                var dialogBoxTrigger = GetComponent<DialogBoxTrigger>();
                dialogBoxTrigger.enabled = true;
                dialogBoxTrigger.ShowWithCallback(selectionResult =>
                {
                    Debug.Log(selectionResult);
                    dialogBoxContainer.SetActive(false);
                    if (selectionResult == 0) // Element number == "OK"
                    {
                        var bp = new BrowserProperties();
                        bp.title = "Select Save Location";
                        bp.filter = "MPEG-4 Part 14 (*.mp4)|*.mp4";
                        bp.filterIndex = 2;

                        new FileBrowser().SaveFileBrowser(bp, getSaveFilename(), "mp4", pathResult =>
                        {
                            PathConfig.saveFolder = pathResult;
                            ScreenRecorder.takeVideo = true;

                            // Disable btns and restart music
                            Main.shouldDisableBtns = 1;
                            Main.audioSource.Stop();
                            Main.audioSource.Play();

                            Debug.Log(pathResult);
                        });
                    }
                });
            }
        }
        else if (togglePicture.isOn)
        {
            if (AddImagesToGrid.replacedChildrenWithSingle)
            {
                var bp = new BrowserProperties();
                bp.title = "Select Save Location";
                bp.filter = "Portable Network Graphics (*.png)|*.png";
                bp.filterIndex = 2;

                new FileBrowser().SaveFileBrowser(bp, getSaveFilename(), "png", result =>
                {
                    Texture2D currentPictureTex = (Texture2D)GameObject.Find("SpiralPuzzlePanel").GetComponentInChildren<RawImage>().texture; // pull in our file data bytes for the specified image format (has to be done from main thread)
                    byte[] fileData = currentPictureTex.EncodeToPNG();

                    // create new thread to save the image to file (only operation that can be done in background)
                    new System.Threading.Thread(() =>
                        {
                            var f = System.IO.File.Create(result);

                            f.Write(fileData, 0, fileData.Length);
                            f.Close();
                            Debug.Log(string.Format("Wrote screenshot {0} of size {1}", result, fileData.Length));
                        }).Start();

                    Debug.Log(result);
                });
            }
            else
            {

            }
        }
    }


    private IEnumerator ChangeFileDropDownValue(int newValue)
    {
        fileDropDown.Select();
        yield return new WaitForEndOfFrame();
        fileDropDown.value = newValue;
        fileDropDown.RefreshShownValue();

        yield break;
    }

    private string getSaveFilename()
    {
        string currentAudioFilename = Main.selectedAudioPathName.Replace(Path.GetExtension(Main.selectedAudioPathName), "");
        return String.Format("The Colour of Music - {0} - {1}",
        currentAudioFilename,
        DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss"));
    }
}
