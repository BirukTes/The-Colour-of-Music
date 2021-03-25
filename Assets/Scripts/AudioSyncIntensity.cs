using System.Collections;
using UnityEngine;
using Light2DE = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class AudioSyncIntensity : MonoBehaviour
{
    public Light2DE pointLight2D;
    public float restIntensity = 1f;
    //The start and finish positions for the interpolation
    private float startIntensity;
    private float endIntensity;
    public float timeTakenDuringLerp = 0.5f;
    //Whether we are currently interpolating or not
    private bool isLerping;
    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    // private float previousInten
    void StartLerping(float targetIntensity)
    {
        isLerping = true;
        timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        startIntensity = pointLight2D.intensity;
        endIntensity = targetIntensity;
    }

    void Update()
    {
        if (Main.audioSource.isPlaying)
        {
            // Normalised target tempo/Intensity
            float targetIntensity = AudioProcessor.currentTempoValue / 50;
            Debug.Log("tempo" + targetIntensity);
            
            if (pointLight2D.intensity != targetIntensity)
                StartLerping(targetIntensity);
        }
        else
        {
            if (pointLight2D.intensity != restIntensity)
            {
                StartLerping(restIntensity);
            }
        }
    }

    // We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (isLerping)
        {
            //We want percentage = 0.0 when Time.time = timeStartedLerping
            //and percentage = 1.0 when Time.time = timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - timeStartedLerping" is.
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = (timeSinceStarted * endIntensity) / timeTakenDuringLerp;

            //to start another lerp)
            pointLight2D.intensity = Mathf.Lerp(startIntensity, endIntensity, percentageComplete);

            //When we've completed the lerp, we set isLerping to false
            if (percentageComplete >= 0.999f)
            {
                isLerping = false;
            }
        }
    }
}
