using System;
using System.Collections.Generic;

/// 
/// <summary>
/// @author MM <mushfiqazeri@gmail.com>
/// Ref: https://www.mycertnotes.com/en/spiral-matrix-clockwise-and-counter-clockwise/
/// </summary>
public class SpiralMatrix
{

    //  Main driver for Testing
    //public static void Main(string[] args)
    //{
    //    Console.WriteLine("Please enter matrix lengths (example 4 3):\n");

    //    Console.WriteLine("Row:");
    //    int row = Convert.ToInt32(Console.ReadLine());
    //    Console.WriteLine("Column:");
    //    int column = Convert.ToInt32(Console.ReadLine());

    //    var mat = SpiralMatrix.createMatrix(row, column);
    //    for (int i = 0; i < mat.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < mat.GetLength(1); j++)
    //        {
    //            Console.Write(mat[i, j] + "\t");
    //        }
    //        Console.WriteLine();
    //    }
    //    Console.WriteLine("\n\n");
    //    var path = SpiralMatrix.spiralMatrixClockwise(mat, row, column);
    //    for (int j = 0; j < path.Count; j++)
    //    {
    //        Console.Write(path[j] + "\t");
    //    }


    //    Console.WriteLine("\n\n\n\n");
    //    path = SpiralMatrix.spiralMatrixCounterClockwise(mat, row, column);
    //    for (int j = 0; j < path.Count; j++)
    //    {
    //        Console.Write(path[j] + "\t");
    //    }


    //    Console.ReadLine();
    //}

    public static List<int> spiralMatrixClockwise(int[,] matrix, int row, int column)
    {
        // Console.WriteLine("\nClockwise elements:");
        List<int> spiralPath = new List<int>();

        int left = (column % 2 == 0) ? (column / 2 - 1) : (column / 2);
        int right = left + 1;
        int top = (row % 2 == 0) ? (row / 2 - 1) : (row / 2);
        int bottom = top + 1;


        // Self-invoked local function
        new Action(() =>
        {
            while (true)
            {
                for (int i = left; i < right; i++)
                {
                    (bool match, int value) = getNextSpiralElement(top, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                left--;

                for (int i = top; i < bottom; i++)
                {
                    (bool match, int value) = getNextSpiralElement(i, right, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                top--;

                for (int i = right; i > left; i--)
                {
                    (bool match, int value) = getNextSpiralElement(bottom, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                right++;

                for (int i = bottom; i > top; i--)
                {
                    (bool match, int value) = getNextSpiralElement(i, left, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                bottom++;
            }
        })();


        return spiralPath;
    }

    public static List<int> spiralMatrixCounterClockwise(int[,] matrix, int row, int column)
    {
        List<int> spiralPath = new List<int>();

        int right = column / 2;
        int left = right - 1;
        int top = (row % 2 == 0) ? (row / 2 - 1) : (row / 2);
        int bottom = top + 1;

        // Self-invoked local function
        new Action(() =>
        {
            while (true)
            {
                for (int i = right; i > left; i--)
                {
                    (bool match, int value) = getNextSpiralElement(top, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                right++;

                for (int i = top; i < bottom; i++)
                {
                    (bool match, int value) = getNextSpiralElement(i, left, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                top--;

                for (int i = left; i < right; i++)
                {
                    (bool match, int value) = getNextSpiralElement(bottom, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                left--;

                for (int i = bottom; i > top; i--)
                {
                    (bool match, int value) = getNextSpiralElement(i, right, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                bottom++;
            }
        })();

        return spiralPath;
    }

    public static int[,] createMatrix(int row, int column)
    {
        int[,] matrix = new int[row, column];
        int value = 1;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = value++;
            }
        }

        return matrix;
    }

    private static (bool, int) getNextSpiralElement(int i, int j, int[,] matrix, int row, int column)
    {
        if (i < 0 || i >= row || j < 0 || j >= column)
        {
            return (false, 0);
        }

        return (true, matrix[i, j]);
    }
}

/// <summary>
/// @author MM &lt;mushfiqazeri@gmail.com&gt;
/// Ref: https://www.mycertnotes.com/en/spiral-matrix-clockwise-and-counter-clockwise/
/// </summary>
public class CopyOfSpiralMatrix
{

    //  Main driver for Testing
    //public static void Main(string[] args)
    //{
    //    Console.WriteLine("Please enter matrix lengths (example 4 3):\n");

    //    Console.WriteLine("Row:");
    //    int row = Convert.ToInt32(Console.ReadLine());
    //    Console.WriteLine("Column:");
    //    int column = Convert.ToInt32(Console.ReadLine());

    //    var mat = CopyOfSpiralMatrix.createMatrix(row, column);
    //    for (int i = 0; i < mat.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < mat.GetLength(1); j++)
    //        {
    //            Console.Write(mat[i, j] + "\t");
    //        }
    //        Console.WriteLine();
    //    }
    //    Console.WriteLine("\n\n");
    //    var path = CopyOfSpiralMatrix.spiralMatrixClockwise(mat, row, column);
    //    for (int j = 0; j < path.Count; j++)
    //    {
    //        Console.Write(path[j] + "\t");
    //    }


    //    Console.WriteLine("\n\n\n\n");
    //    path = CopyOfSpiralMatrix.spiralMatrixCounterClockwise(mat, row, column);
    //    for (int j = 0; j < path.Count; j++)
    //    {
    //        Console.Write(path[j] + "\t");
    //    }


    //    Console.ReadLine();
    //}

    public static List<int> spiralMatrixClockwise(int[,] matrix, int row, int column)
    {
        // Console.WriteLine("\nClockwise elements:");
        List<int> spiralPath = new List<int>();

        int left = (column % 2 == 0) ? (column / 2 - 1) : (column / 2);
        int right = left + 1;
        int top = (row % 2 == 0) ? (row / 2 - 1) : (row / 2);
        int bottom = top + 1;


        // Self-invoked local function
        new Action(() =>
        {
            while (true)
            {
                for (int i = left; i < right; i++)
                {
                    (bool match, int value) = getNextSpiralElement(top, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                left--;

                for (int i = top; i < bottom; i++)
                {
                    (bool match, int value) = getNextSpiralElement(i, right, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                top--;

                for (int i = right; i > left; i--)
                {
                    (bool match, int value) = getNextSpiralElement(bottom, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                right++;

                for (int i = bottom; i > top; i--)
                {
                    (bool match, int value) = getNextSpiralElement(i, left, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                bottom++;
            }
        })();


        return spiralPath;
    }

    public static List<int> spiralMatrixCounterClockwise(int[,] matrix, int row, int column)
    {
        List<int> spiralPath = new List<int>();

        int right = column / 2;
        int left = right - 1;
        int top = (row % 2 == 0) ? (row / 2 - 1) : (row / 2);
        int bottom = top + 1;

        // Self-invoked local function
        new Action(() =>
        {
            while (true)
            {
                for (int i = right; i > left; i--)
                {
                    (bool match, int value) = getNextSpiralElement(top, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                right++;

                for (int i = top; i < bottom; i++)
                {
                    (bool match, int value) = getNextSpiralElement(i, left, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                top--;

                for (int i = left; i < right; i++)
                {
                    (bool match, int value) = getNextSpiralElement(bottom, i, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                left--;

                for (int i = bottom; i > top; i--)
                {
                    (bool match, int value) = getNextSpiralElement(i, right, matrix, row, column);
                    if (match)
                    {
                        spiralPath.Add(value);
                    }
                    else
                    {
                        return;
                    }
                }
                bottom++;
            }
        })();

        return spiralPath;
    }

    public static int[,] createMatrix(int row, int column)
    {
        int[,] matrix = new int[row, column];
        int value = 1;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = value++;
            }
        }

        return matrix;
    }

    private static (bool, int) getNextSpiralElement(int i, int j, int[,] matrix, int row, int column)
    {
        if (i < 0 || i >= row || j < 0 || j >= column)
        {
            return (false, 0);
        }

        return (true, matrix[i, j]);
    }
}
