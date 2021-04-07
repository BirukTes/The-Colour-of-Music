using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private static Slider sliderBar;
    private static GameObject progressBarBackgroundGO;

    public static void EnableProgressBarGO()
    {
        if (progressBarBackgroundGO == null)
        {
            progressBarBackgroundGO = GameObject.Find("MainCanvas").transform.Find("ProgressBarBackground").gameObject;
            progressBarBackgroundGO.SetActive(true);

            sliderBar = progressBarBackgroundGO.transform.Find("ProgressBar").GetComponent<Slider>();
        } else {
            progressBarBackgroundGO.SetActive(true);
        }
    }

    public static void DisableProgressBarGO()
    {
        if (progressBarBackgroundGO != null)
            if (progressBarBackgroundGO.activeSelf)
                progressBarBackgroundGO.SetActive(false);
    }

    public static void IncrementProgressBar(float value)
    {
        sliderBar.value = value;
    }
}
