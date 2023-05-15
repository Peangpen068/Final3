using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the input image file path:");
        string inputImagePath = Console.ReadLine();

        Console.WriteLine("Enter the convolution kernel file path:");
        string kernelFilePath = Console.ReadLine();

        Console.WriteLine("Enter the output image file path:");
        string outputImagePath = Console.ReadLine();

        double[,] imageData = ReadImageDataFromFile(inputImagePath);
        double[,] kernelData = ReadImageDataFromFile(kernelFilePath);

        double[,] convolvedImage = Convolve(imageData, kernelData);

        WriteImageDataToFile(outputImagePath, convolvedImage);

        Console.WriteLine("Convolution completed. Result saved to: " + outputImagePath);
    }

    static double[,] ReadImageDataFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.Length;
        int cols = lines[0].Split(' ').Length;

        double[,] imageData = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] values = lines[i].Split(' ');

            for (int j = 0; j < cols; j++)
            {
                imageData[i, j] = Convert.ToDouble(values[j]);
            }
        }

        return imageData;
    }

    static double[,] Convolve(double[,] imageData, double[,] kernel)
    {
        int imageRows = imageData.GetLength(0);
        int imageCols = imageData.GetLength(1);
        int kernelSize = kernel.GetLength(0);

        int padding = kernelSize / 2;
        int outputRows = imageRows + 2 * padding;
        int outputCols = imageCols + 2 * padding;

        double[,] outputImage = new double[outputRows, outputCols];

        for (int i = 0; i < imageRows; i++)
        {
            for (int j = 0; j < imageCols; j++)
            {
                for (int ki = 0; ki < kernelSize; ki++)
                {
                    for (int kj = 0; kj < kernelSize; kj++)
                    {
                        int rowIndex = i + ki - padding;
                        int colIndex = j + kj - padding;

                        if (rowIndex < 0 || rowIndex >= imageRows)
                            rowIndex = Math.Abs(rowIndex % imageRows);

                        if (colIndex < 0 || colIndex >= imageCols)
                            colIndex = Math.Abs(colIndex % imageCols);

                        outputImage[i, j] += imageData[rowIndex, colIndex] * kernel[ki, kj];
                    }
                }
            }
        }

        return outputImage;
    }

    static void WriteImageDataToFile(string filePath, double[,] imageData)
    {
        int rows = imageData.GetLength(0);
        int cols = imageData.GetLength(1);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    writer.Write(imageData[i, j] + " ");
                }
                writer.WriteLine();
            }
        }
    }
}

