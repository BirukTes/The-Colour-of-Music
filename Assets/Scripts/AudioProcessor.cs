using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioProcessor : MonoBehaviour
{
    private static SonicRunner sonic = new SonicRunner();
    public static string currentEmotionValue { get; private set; }

    private static IList<TransformerDataResult> tempoDataResults, modeDataResults;

    //Make sure your GameObject has an AudioSource component first
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the AudioSource from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();

        setAudioData();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            float currentTimePosition = audioSource.time;
            // Debug.Log(currentTimePosition);
            getEmotion(currentTimePosition);
        }

    }

    public static void setAudioData()
    {
        // audioSource.clip.
        (int exitCode, string jsonStrTempiOutput) = sonic.sonicGetTempi(Main.selectedAudioPath);

        if (exitCode == 0)
        {
            // Call the deserializer  
            tempoDataResults = JsonHelper.DeserializeToList<TransformerDataResult>(jsonStrTempiOutput);
        }

        (int exitCode1, string jsonStrModeOutput) = sonic.sonicGetModes(Main.selectedAudioPath);

        if (exitCode1 == 0)
        {
            modeDataResults = JsonHelper.DeserializeToList<TransformerDataResult>(jsonStrModeOutput);
        }
    }

    private void getEmotion(float currentTimePosition)
    {
        // currentEmotionValue = "None";
        float tempo = 0f;
        int mode = 0; // 0: minor, 1:major        

        foreach (var tempoData in tempoDataResults)
        {
            if (tempoData.Time >= currentTimePosition && tempoData.Time <= currentTimePosition)
            {
                tempo = tempoData.Value;
                break;
            }
        }
        foreach (var modeData in modeDataResults)
        {
            if (modeData.Time >= currentTimePosition && modeData.Time <= currentTimePosition)
            {
                mode = (int)modeData.Value;
                break;
            }
        }

        if ((tempo > 108) && (mode == 1))
        {
            currentEmotionValue = "happy";
        }
        else if ((tempo < 108) && (mode == 0))
        {
            currentEmotionValue = "sad";
        }
    }

}

