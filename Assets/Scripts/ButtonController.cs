using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using System.IO;

public class ButtonController : MonoBehaviour
{
    public Toggle ToggleVideo;
    public Toggle TogglePicture;

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
            Main.audioSource.Play();
        }
    }

    public void SelectAudioFile()
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

    public void ToggleMovieOrPicture()
    {
        if (ToggleVideo.isOn)
        {
            GetComponent<AudioProcessor>().enabled = true;
            GetComponent<AudioSyncColor>().enabled = true;
            GetComponent<AddImagesGrid>().enabled = false;
        }
        else if (TogglePicture.isOn)
        {
            GetComponent<AudioProcessor>().enabled = false;
            GetComponent<AudioSyncColor>().enabled = false;
            GetComponent<AddImagesGrid>().enabled = true;
        }
    }


    public void SaveVisualisation()
    {
        if (ToggleVideo.isOn)
        {
            // TODO
        }
        else if (TogglePicture.isOn)
        {

            var bp = new BrowserProperties();
            bp.title = "Select Audio File";
            bp.filter = "Supported Image File (*.png)|*.png";
            bp.filterIndex = 2;

            new FileBrowser().SaveFileBrowser(bp, "The Colour of Music", "png", result =>
            {
                // create new thread to save the image to file (only operation that can be done in background)
                new System.Threading.Thread(() =>
                {
                    Texture2D currentPictureTex = (Texture2D)GameObject.Find("SpiralPuzzlePanel").GetComponentInChildren<RawImage>().texture;

                    // pull in our file data bytes for the specified image format (has to be done from main thread)
                    byte[] fileData = currentPictureTex.EncodeToPNG();    // create file and write optional header with image bytes
                    var f = System.IO.File.Create(result);

                    f.Write(fileData, 0, fileData.Length);
                    f.Close();
                    Debug.Log(string.Format("Wrote screenshot {0} of size {1}", result, fileData.Length));
                }).Start();

                Debug.Log(result);
            });
        }
    }
}
