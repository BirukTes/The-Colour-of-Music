using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddImagesToGrid : MonoBehaviour
{
    public GameObject puzzleField;
    public GameObject ControlsGO;
    public RawImage rawImage;

    // Bools
    private static bool finishedSettingPlaceholders = false;
    private static bool finishedSettingTextures = false;
    private static bool calculatedRows = false;
    private static bool finishedMainRoutine = false;
    public static bool replacedChildrenWithSingle = false;


    private List<int> spiralPath;
    private Texture2D screenShotTexture;
    private int screenShotsCount = 0;
    private float overallAmountToProcess = 0;
    private float overallProcessedAmount = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        puzzleField.SetActive(true);

        if (!replacedChildrenWithSingle)
        {
            ProgressBar.EnableProgressBarGO();
            foreach (Transform child in puzzleField.transform)
            {
                Destroy(child.gameObject);
            }


            if (!finishedSettingPlaceholders)
            {
                //Change the path to location where your images are stored.
                if (ScreenRecorder.screenShotList != null)
                {
                    Main.addImagesToGridisProcessing = true;

                    screenShotsCount = ScreenRecorder.screenShotList.Count;
                    overallAmountToProcess = ((float)screenShotsCount) * 2; // For both placeholder and adding images process
                    StartCoroutine("addPlaceholders");
                }
            }
        }
    }

    void OnDisable()
    {
        puzzleField.SetActive(false);
    }

    void Update()
    {
        if (puzzleField.activeSelf)
        {
            if (finishedSettingPlaceholders)
            {
                if (!finishedSettingTextures)
                {
                    StartCoroutine("addRawImages");
                }
                else
                {
                    if (!finishedMainRoutine)
                    {
                        Debug.Log("Finished Adding Images");
                        StopCoroutine("addRawImages");
                        StartCoroutine("takeScreenShot");
                    }
                    else if (!replacedChildrenWithSingle)
                    {
                        StopCoroutine("takeScreenShot");

                        StartCoroutine("replaceChildrenWithSingleImage");
                    }
                }
            }
        }
    }


    //////////////////////// Coroutines ///////////////////
    private IEnumerator addPlaceholders()
    {
        for (int i = 0; i < screenShotsCount; i++)
        {
            RawImage currentRawImageComp = Instantiate(rawImage);
            currentRawImageComp.transform.SetParent(puzzleField.transform, false);

            overallProcessedAmount = (float)i;
            ProgressBar.IncrementProgressBar(overallProcessedAmount / overallAmountToProcess);
            yield return null;
        }
        finishedSettingPlaceholders = true;
        yield break;
    }

    private IEnumerator addRawImages()
    {
        if (!calculatedRows)
        {
            float squareRoot = Mathf.Sqrt(puzzleField.transform.childCount);
            int row = Mathf.CeilToInt(squareRoot);
            int column = row;

            // Get diff between children
            int diffBetweenChild = Mathf.Abs(((row * column) - puzzleField.transform.childCount));
            int amountByToReduce = 0;

            if (diffBetweenChild != 0)
            {
                // Get diff between Rows
                int diffBetweenRow = Mathf.Abs(((row * column) - ((row - 1) * column)));

                // Find out if the row diff between children and rows is only 1 row, is minus 1
                if (diffBetweenChild == diffBetweenRow)
                {
                    // If so, take 1 from the row, otherwise ...
                    // This will make the code efficient reducing the need to calculate redundant rows
                    row -= 1;
                }
                else if (diffBetweenChild > row)
                {
                    // Otherwise, reduce from spiralPath variable, by passing the amount needed to reduce
                    // Row and Amount reduction are needed
                    amountByToReduce = diffBetweenChild - row;
                    row -= 1;
                }
                else
                {
                    amountByToReduce = diffBetweenChild;
                }
            }
            calculateNum(row, column, amountByToReduce);
            calculatedRows = true;
            Debug.Log("Done");
        }

        for (int i = 0; i < spiralPath.Count; i++)
        {
            // path begins from 1, but children from 0
            RawImage currentRawImageComp = puzzleField.transform.GetChild(spiralPath[i] - 1).GetComponent<RawImage>();
            ScreenRecorder.screenShotList[i].Compress(false);
            currentRawImageComp.texture = ScreenRecorder.screenShotList[i];

            ProgressBar.IncrementProgressBar((overallProcessedAmount + (float)i) / overallAmountToProcess);
            yield return null;
        }

        finishedSettingTextures = true;
        yield break;
    }

    private IEnumerator takeScreenShot()
    {

        ControlsGO.SetActive(false);
        ProgressBar.DisableProgressBarGO();

        yield return new WaitForEndOfFrame();


        if (!ControlsGO.activeSelf)
        {
            screenShotTexture = ScreenCapture.CaptureScreenshotAsTexture();
            finishedMainRoutine = true;
            ControlsGO.SetActive(true);
        }

        yield break;
    }
    private IEnumerator replaceChildrenWithSingleImage()
    {
        for (int i = 0; i < puzzleField.transform.childCount - 1; i++)
        {
            Destroy(puzzleField.transform.GetChild(i).gameObject);
        }

        yield return new WaitForEndOfFrame();

        // path begins from 1, but children from 0
        RawImage currentRawImageComp = puzzleField.transform.GetChild(0).GetComponent<RawImage>();
        // ScreenRecorder.screenShotList[i].Compress(false);
        currentRawImageComp.texture = screenShotTexture;
        replacedChildrenWithSingle = true;
        Main.addImagesToGridisProcessing = false;

        yield break;
    }


    //////////////////////// Functions //////////////////////
    private void calculateNum(int row, int column, int amountByToReduce)
    {
        int[,] spiralMatrix;

        Debug.Log("Done: " + row + " " + column);

        spiralMatrix = SpiralMatrix.createMatrix(row, column);

        spiralPath = SpiralMatrix.spiralMatrixCounterClockwise(spiralMatrix, row, column);

        if (amountByToReduce != 0)
        {
            IEnumerable<int> range = System.Linq.Enumerable.Range((spiralPath.Count + 1) - amountByToReduce, amountByToReduce);

            foreach (int num in range)
            {
                spiralPath.Remove(num);
            }
        }
    }


    //// RESET VALUES    
    public static void ResetValues()
    {
        finishedSettingPlaceholders = false;
        finishedSettingTextures = false;
        calculatedRows = false;
        finishedMainRoutine = false;
        replacedChildrenWithSingle = false;
    }
}
