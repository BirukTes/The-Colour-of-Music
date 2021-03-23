using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryIntroPanel : MonoBehaviour
{

    public GameObject introPanel;
    public CanvasGroup canvasGroup;

    //The start and finish positions for the interpolation
    private float startAlpha;
    private float endAlpha;
    public float timeTakenDuringLerp = 0.3f;
    //Whether we are currently interpolating or not
    private bool isLerping;
    //The Time.time value when we started the interpolation
    private float timeStartedLerping;
    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    // private float previousInten
    void OnEnable()
    {
        isLerping = true;
        timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        startAlpha = canvasGroup.alpha;
        endAlpha = 0;
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
            float percentageComplete = (timeSinceStarted) / timeTakenDuringLerp;

            //to start another lerp)
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, percentageComplete);

            //When we've completed the lerp, we set isLerping to false
            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
                Destroy(introPanel);
                this.enabled = false;
            }
        }
    }
}
