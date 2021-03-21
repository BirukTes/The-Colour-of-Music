using System.Collections;
using UnityEngine;
using Light2DE = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class AudioSyncColor : MonoBehaviour
{
    public Color[] emotionColours;
    public Color restColor;
    public Light2DE pointLight2D;

    //The start and finish positions for the interpolation
    private Color startColour;
    private Color endColour;

    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;
    //Whether we are currently interpolating or not
    private bool isLerping;
    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    private float tempoMultiplier;
    /// <summary>
    /// Called to begin the linear interpolation
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
            Color currentColour = emotionColours[0];

            if (AudioProcessor.currentEmotionValue == "sad")
            {
                currentColour = emotionColours[1];
            }
            // StopCoroutine("MoveToColor"); TODO: guard with prevColour, if not changed donot change


            if (pointLight2D.color != currentColour)
            {
                // StartCoroutine("MoveToColor1", currentColour);
                tempoMultiplier = AudioProcessor.currentTempoValue / 50;

                StartLerping(currentColour);
            }
            // // pointLight2D.color = Color.Lerp(pointLight2D.color, restColor, 0.5f * Time.deltaTime);
            // MoveToColor(currentColour);

        }
        else
        {
            if (pointLight2D.color != restColor)
            {
                StartLerping(restColor);
            }
        }
    }
    // private IEnumerator MoveToColor(Color targetColour)
    // {
    //     Color currentColour = pointLight2D.color;
    //     Color initialColour = currentColour;
    //     float timer = 0.5f;

    //     while (currentColour != targetColour)
    //     {
    //         currentColour = Color.Lerp(initialColour, targetColour, timer * Time.deltaTime);
    //         // timer += Time.deltaTime;

    //         pointLight2D.color = currentColour;

    //         yield return null;
    //     }
    // }
    private IEnumerator MoveToColor1(Color targetColour)
    {
        Color currentColour = pointLight2D.color;
        Color initialColour = currentColour;
        float timer = 0;
        float totalTime = 2f;

        while (timer < totalTime)
        {
            currentColour = Color.Lerp(initialColour, targetColour, timer / totalTime);
            timer += Time.deltaTime;

            pointLight2D.color = currentColour;

            yield return new WaitForEndOfFrame();
        }
    }
    private void MoveToColor(Color targetColour)
    {
        Color currentColour = pointLight2D.color;
        Color initialColour = currentColour;
        float timer = 1f;

        currentColour = Color.Lerp(initialColour, targetColour, timer * Time.deltaTime);
        // timer += Time.deltaTime;

        pointLight2D.color = currentColour;
    }


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
            float percentageComplete = (timeSinceStarted * tempoMultiplier) / timeTakenDuringLerp;

            //to start another lerp)
            pointLight2D.color = Color.Lerp(startColour, endColour, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
        }
    }
}
