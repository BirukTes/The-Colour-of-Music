using System.Collections;
using UnityEngine;
using Light2DE = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class AudioSyncIntensity : MonoBehaviour
{
    public Light2DE pointLight2D;
    private float smoothTime = 2f;
    //The start and finish positions for the interpolation
    private float startIntensity;
    private float endIntensity;
    public float timeTakenDuringLerp = 1f;
    //Whether we are currently interpolating or not
    private bool isLerping;
    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    // private float previousInten
    void StartLerping(float targetColour)
    {
        isLerping = true;
        timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        startIntensity = pointLight2D.intensity;
        endIntensity = targetColour;
    }
    void Update()
    {
        if (Main.audioSource.isPlaying)
        {
            // Normalised target tempo/Intensity
            float targetIntensity = AudioProcessor.currentTempoValue / 50;
            Debug.Log("tempo" + targetIntensity);
            // StopCoroutine("MoveToIntensity");  
            // StartCoroutine(MoveToIntensity(targetIntensity));  
            if (pointLight2D.intensity != targetIntensity)
                StartLerping(targetIntensity);

            // MoveToIntensity1(targetIntensity);
        }
    }
    // private IEnumerator MoveToIntensity(float targetIntensity, float tempo)
    // {
    //     float currentIntensity = pointLight2D.intensity;
    //     float initialIntensity = currentIntensity;
    //     float timer = 0;

    //     while (currentIntensity != targetIntensity)
    //     {
    //         currentIntensity = Mathf.Lerp(initialIntensity, targetIntensity, timer / targetIntensity);
    //         timer += Time.deltaTime;

    //         pointLight2D.intensity = currentIntensity;

    //         yield return null;
    //     }
    // }
    private IEnumerator MoveToIntensity(float targetIntensity)
    {
        float currentIntensity = pointLight2D.intensity;
        float initialIntensity = currentIntensity;
        float timer = 0;
        float totalTime = 1f;

        while (timer < totalTime)
        {
            currentIntensity = Mathf.Lerp(initialIntensity, targetIntensity, timer / totalTime);
            timer += Time.deltaTime;

            pointLight2D.intensity = currentIntensity;

            yield return new WaitForEndOfFrame();
        }
    }
    // private void MoveToIntensity1(float targetTempo)
    // {
    //     float currentIntensity = pointLight2D.intensity;
    //     float initialIntensity = currentIntensity;


    //     currentIntensity = Mathf.Lerp(initialIntensity, targetTempo, targetTempo * Time.deltaTime);
    //     // timer += Time.deltaTime;

    //     pointLight2D.intensity = currentIntensity;
    // }



    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = (timeSinceStarted * endIntensity) / timeTakenDuringLerp;

            //to start another lerp)
            pointLight2D.intensity = Mathf.Lerp(startIntensity, endIntensity, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
        }
    }
}
