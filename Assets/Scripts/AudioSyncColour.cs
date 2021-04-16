using System.Collections;
using UnityEngine;
using Light2DE = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class AudioSyncColour : MonoBehaviour
{
    public Color[] emotionColours;
    public Color restColour;
    public Light2DE pointLight2D;

    //The start and finish positions for the interpolation
    private Color startColour;
    private Color endColour;

    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 0.5f;
    //Whether we are currently interpolating or not
    private bool isLerping;
    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    private float tempoMultiplier;
    private Color currentColour;

    /// <summary>
    /// Called to begin the linear interpolation
    /// Ref: https://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly
    /// </summary>
    void StartLerping(Color targetColour)
    {
        isLerping = true;
        timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        startColour = pointLight2D.color;
        endColour = targetColour;
    }

    void Update()
    {
        if (Main.audioSource.isPlaying)
        {
            currentColour = restColour; // could be "None"

            if (AudioProcessor.currentEmotionValue == "happy")
            {
                currentColour = emotionColours[0];
            }
            else if (AudioProcessor.currentEmotionValue == "sad")
            {
                currentColour = emotionColours[1];
            }
            else if (AudioProcessor.currentEmotionValue == "serenity")
            {
                currentColour = emotionColours[2];
            }
            else if (AudioProcessor.currentEmotionValue == "fear/anger")
            {
                currentColour = emotionColours[3];
            }

            if (pointLight2D.color != currentColour)
            {
                // StartCoroutine("MoveToColor1", currentColour);
                tempoMultiplier = AudioProcessor.currentTempoValue / 50;

                StartLerping(currentColour);
            }
        }
        else
        {
            if (pointLight2D.color != restColour)
            {
                StartLerping(restColour);
            }
        }
    }

    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (isLerping)
        {
            //We want percentage = 0.0 when Time.time = timeStartedLerping
            //and percentage = 1.0 when Time.time = timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - timeStartedLerping" is.
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = (timeSinceStarted * tempoMultiplier) / timeTakenDuringLerp;

            //to start another lerp)
            pointLight2D.color = Color.Lerp(startColour, endColour, percentageComplete);

            //When we've completed the lerp, we set isLerping to false
            if (percentageComplete >= 0.999f)
            {
                isLerping = false;
            }
        }
    }
}
