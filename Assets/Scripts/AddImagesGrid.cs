using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddImagesGrid : MonoBehaviour
{
    public GameObject puzzleField;
    public GameObject ControlsGO;
    public RawImage img;
    private FileInfo[] files;


    private bool set1 = false;
    private bool set = false;
    private bool pressed = false;
    private bool calculated = false;
    private bool finishedMainRoutine = false;
    private bool replaceChildrenWithSingle = false;


    private List<int> spiralPath;
    private FlexibleGridLayout flexible;
    private Texture2D screenShotTexture;

    // Start is called before the first frame update
    void Start()
    {
        puzzleField.SetActive(true);
        foreach (Transform child in puzzleField.transform)
        {
            Destroy(child.gameObject);
        }
        if (!set1)
        {
            StartCoroutine("addPlaceholders1");
        }
        flexible = puzzleField.GetComponent<FlexibleGridLayout>();
    }

    void Update()
    {
        if (puzzleField.activeSelf)
        {
            if (set1)
            {
                if (!set)
                {
                    StartCoroutine("addRawImages1");
                }
                else
                {
                    if (!finishedMainRoutine)
                    {
                        Debug.Log("Finished Adding Images");
                        StopCoroutine("addRawImages1");
                        StartCoroutine("takeScreenShot");
                    }
                    else if (!replaceChildrenWithSingle)
                    {
                        StopCoroutine("takeScreenShot");

                        StartCoroutine("replaceChildrenWithSingleImage");
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }


    // private IEnumerator addPlaceholders()
    // {
    //     //Change the path to location where your images are stored.
    //     DirectoryInfo directory = new DirectoryInfo(@"C:\Users\Biruk\Documents\Unity\The Colour of Music\Recordings\");
    //     if (directory != null)
    //     {
    //         files = directory.GetFiles();

    //         for (int i = 0; i < 16; i++)
    //         {
    //             RawImage image1 = Instantiate(img);
    //             image1.name = "img" + i;
    //             image1.transform.SetParent(puzzleField.transform, false);
    //             yield return null;
    //         }
    //     }

    //     set1 = true;
    // }

    // private IEnumerator addRawImages()
    // {
    //     int row = flexible.rows;
    //     int column = flexible.columns;

    //     if (row != 0)
    //     {
    //         if (!calculated)
    //         {
    //             // Get diff between children
    //             int diffBetweenChild = Mathf.Abs(((row * column) - puzzleField.transform.childCount));
    //             int amountByToReduce = 0;

    //             if (diffBetweenChild != 0)
    //             {
    //                 // Get diff between Rows
    //                 int diffBetweenRow = Mathf.Abs(((row * column) - ((row - 1) * column)));

    //                 // Find out if the row diff between children and rows is only 1 row, is minus 1
    //                 if (diffBetweenChild == diffBetweenRow)
    //                 {
    //                     // If so, take 1 from the row, otherwise ...
    //                     // This will make the code efficient reducing the need to calculate redundant rows
    //                     row -= 1;
    //                 }
    //                 else
    //                 {
    //                     // Otherwise, reduce from spiralPath variable, by passing the amount needed to reduce
    //                     amountByToReduce = diffBetweenChild;
    //                 }
    //             }
    //             calculateNum(row, column, amountByToReduce);
    //             calculated = true;
    //             Debug.Log("Done");
    //         }

    //         for (int i = 0; i < spiralPath.Count; i++)
    //         {
    //             // read image and store in a byte array
    //             byte[] byteArray = File.ReadAllBytes(files[i].FullName);
    //             //create a texture and load byte array to it
    //             // Texture size does not matter 
    //             Texture2D sampleTexture = new Texture2D(2, 2, TextureFormat.DXT1, false);
    //             // the size of the texture will be replaced by image size
    //             bool isLoaded = sampleTexture.LoadImage(byteArray);

    //             if (isLoaded)
    //             {
    //                 // path begins from 1, but children from 0
    //                 RawImage image1 = puzzleField.transform.GetChild(spiralPath[i] - 1).GetComponent<RawImage>();
    //                 image1.texture = sampleTexture;
    //             }
    //             yield return null;
    //         }

    //         set = true;
    //     }
    // }

    private IEnumerator addPlaceholders1()
    {
        //Change the path to location where your images are stored.

        if (ScreenRecorder.screenShotList != null)
        {

            for (int i = 0; i < ScreenRecorder.screenShotList.Count; i++)
            {
                RawImage image1 = Instantiate(img);
                image1.name = "img" + i;
                image1.transform.SetParent(puzzleField.transform, false);
                yield return null;
            }
        }

        set1 = true;
    }

    private IEnumerator addRawImages1()
    {
        int row = flexible.rows;
        int column = flexible.columns;

        if (row != 0)
        {
            if (!calculated)
            {
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
                    else
                    {
                        // Otherwise, reduce from spiralPath variable, by passing the amount needed to reduce
                        amountByToReduce = diffBetweenChild;
                    }
                }
                calculateNum(row, column, amountByToReduce);
                calculated = true;
                Debug.Log("Done");
            }

            for (int i = 0; i < spiralPath.Count; i++)
            {
                Debug.Log(puzzleField.transform.childCount);
                Debug.Log("here: " + i + " And her: " + (spiralPath[i] - 1));

                // path begins from 1, but children from 0
                RawImage image1 = puzzleField.transform.GetChild(spiralPath[i] - 1).GetComponent<RawImage>();
                ScreenRecorder.screenShotList[i].Compress(false);
                image1.texture = ScreenRecorder.screenShotList[i];

                yield return null;
            }

            set = true;
        }
    }

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


    private IEnumerator takeScreenShot()
    {

        ControlsGO.SetActive(false);

        yield return new WaitForEndOfFrame();


        if (!ControlsGO.activeSelf)
        {
            screenShotTexture = ScreenCapture.CaptureScreenshotAsTexture();
            finishedMainRoutine = true;
            ControlsGO.SetActive(true);
        }
    }
    private IEnumerator replaceChildrenWithSingleImage()
    {
        for (int i = 0; i < puzzleField.transform.childCount - 1; i++)
        {
            Destroy(puzzleField.transform.GetChild(i).gameObject);
        }

        yield return new WaitForEndOfFrame();

        // path begins from 1, but children from 0
        RawImage image1 = puzzleField.transform.GetChild(0).GetComponent<RawImage>();
        // ScreenRecorder.screenShotList[i].Compress(false);
        image1.texture = screenShotTexture;
        replaceChildrenWithSingle = true;
    }
}
