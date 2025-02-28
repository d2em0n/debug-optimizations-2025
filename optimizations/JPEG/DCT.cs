using System;
using JPEG.Utilities;

namespace JPEG;

public class DCT
{
    private static readonly double[,] CosTable = new double[8, 8];

    static DCT()
    {
        for (var i = 0; i < 8; i++)
        for (var j = 0; j < 8; j++)
            CosTable[i, j] = Math.Cos(((2 * j + 1) * i * Math.PI) / 16);
    }

    public static double[,] DCT2D(double[,] input)
    {
        var height = input.GetLength(0);
        var width = input.GetLength(1);
        var coeffs = new double[width, height];
        var beta = Beta(height, width);

        for (var u = 0; u < width; u++)
        {
            for (var v = 0; v < height; v++)
            {
                var sum = 0d;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        sum += input[x, y] * CosTable[u, x] * CosTable[v, y];
                    }
                }
                coeffs[u, v] = sum * beta * Alpha(u) * Alpha(v);
            }
        }
        return coeffs;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        var height = coeffs.GetLength(0);
        var width = coeffs.GetLength(1);
        var beta = Beta(height, width);
        for (var x = 0; x <width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var sum = 0d;
                for (var u = 0; u < width; u++)
                {
                    for (var v = 0; v < height; v++)
                    {
                        sum += coeffs[u, v] * CosTable[u, x] * CosTable[v, y] *
                            Alpha(u) * Alpha(v);
                    }
                }
                output[x, y] = sum * beta;
            }
        }
    }

    private static double Alpha(int u)
    {
        if (u == 0)
            return 1 / Math.Sqrt(2);
        return 1;
    }

    private static double Beta(int height, int width)
    {
        return 1d / width + 1d / height;
    }
}