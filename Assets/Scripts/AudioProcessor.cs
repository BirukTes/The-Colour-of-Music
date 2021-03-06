using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProcessor : MonoBehaviour
{
    private static SonicRunner sonic = new SonicRunner();
    public static string currentEmotionValue { get; private set; }
    public static float currentTempoValue { get; private set; }

    private static IList<TransformerDataResult> tempoDataResults = new List<TransformerDataResult>(), modeDataResults = new List<TransformerDataResult>();
    private static bool finishedSettingDatasets = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetAudioData(true));
    }

    // Update is called once per frame
    void Update()
    {
        if (Main.audioSource.isPlaying)
        {
            float currentTimePosition = Main.audioSource.time;
            // Debug.Log(currentTimePosition);
            GetEmotion(currentTimePosition);
        }

    }

    public static IEnumerator SetAudioData(bool usePbar)
    {
        if (!finishedSettingDatasets)
        {
            if (usePbar)
            {
                ProgressBar.EnableProgressBarGO();
                ProgressBar.IncrementProgressBar(0.1f);
            }

            // audioSource.clip.
            (int exitCode, string jsonStrTempiOutput) = sonic.sonicGetTempi(Main.audioPathInUseForProcessing);

            if (exitCode == 0)
            {
                // Call the deserializer  
                tempoDataResults = JsonHelper.DeserializeToList<TransformerDataResult>(jsonStrTempiOutput);
                yield return null;
            }

            if (usePbar)
            {
                ProgressBar.IncrementProgressBar(0.5f);
            }

            (int exitCode1, string jsonStrModeOutput) = sonic.sonicGetModes(Main.audioPathInUseForProcessing);

            if (exitCode1 == 0)
            {
                modeDataResults = JsonHelper.DeserializeToList<TransformerDataResult>(jsonStrModeOutput);
                yield return null;
            }

            if (usePbar)
            {
                ProgressBar.IncrementProgressBar(1f);
            }
            ProgressBar.DisableProgressBarGO();

            finishedSettingDatasets = true;
        }
        yield break;
    }

    private void GetEmotion(float currentTimePosition)
    {
        currentEmotionValue = "none";
        currentTempoValue = 0f;
        int mode = 0; // 0:major, 1: minor  
        // Ref:http://vamp-plugins.org/rdf/plugins/qm-vamp-plugins#qm-keydetector     

        currentTempoValue = getValueFor(tempoDataResults, currentTimePosition);
        mode = (int)getValueFor(modeDataResults, currentTimePosition);


        if ((currentTempoValue > 108) && (mode == 0))
        {
            currentEmotionValue = "happy";
        }
        else if ((currentTempoValue < 108) && (mode == 0))
        {
            currentEmotionValue = "serenity";
        }
        else if ((currentTempoValue > 184) && (mode == 1))
        {
            currentEmotionValue = "fear/anger";
        }
        else if ((currentTempoValue < 108) && (mode == 1))
        {
            currentEmotionValue = "sad";
        }
    }

    private float getValueFor(IList<TransformerDataResult> transformerDataResults, float currentTimePosition)
    {
        int loopTo = transformerDataResults.Count;
        for (int i = 0; i < loopTo; i++)
        {
            if (((loopTo == 1) || (i + 1) > (loopTo - 1)))
            {
                // This is the end of the music/timeline, either there is one count or i + 1 will be out of range.
                return transformerDataResults[i].Value;
            }
            else
            {
                if (currentTimePosition >= transformerDataResults[i].Time && currentTimePosition <= transformerDataResults[i + 1].Time)
                {
                    return transformerDataResults[i].Value;
                }
            }
        }
        return 0;
    }


    //// RESET VALUES    
    public static void ResetValues()
    {
        finishedSettingDatasets = false;
        currentTempoValue = 0f;
        tempoDataResults = new List<TransformerDataResult>();
        modeDataResults = new List<TransformerDataResult>();
    }

}

