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
        var alpha = 1 / Math.Sqrt(2);
        
        //u==0 v==0
        var sum = 0d;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                sum += input[x, y] * CosTable[0, x] * CosTable[0, y];
            }
        }
        coeffs[0, 0] = sum * beta * alpha * alpha;
        
        //u==0 v!=0
        for (var v = 1; v < height; v++)
        {
            sum = 0d;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    sum += input[x, y] * CosTable[0, x] * CosTable[v, y];
                }
            }
            coeffs[0, v] = sum * beta * alpha;
        }

        //u!=0 v==0
        for (var u = 1; u < width; u++)
        {
            sum = 0d;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    sum += input[x, y] * CosTable[u, x] * CosTable[0, y];
                }
            }
            coeffs[u, 0] = sum * beta * alpha;
        }

        // u!=0  v!=0
        for (var u = 1; u < width; u++)
        {
            for (var v = 1; v < height; v++)
            {
                sum = 0d;
                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        sum += input[x, y] * CosTable[u, x] * CosTable[v, y];
                    }
                }
                coeffs[u, v] = sum * beta;
            }
        }
        return coeffs;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        var height = coeffs.GetLength(0);
        var width = coeffs.GetLength(1);
        var beta = Beta(height, width);
        var alpha = 1 / Math.Sqrt(2);
        
        for (var x = 0; x <width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var sum = 0d;
                
                //u==0 v==0
                sum += coeffs[0, 0] * CosTable[0, x] * CosTable[0, y] * alpha * alpha; 

                // u==0 v!=0
                for (var v = 1; v < height; v++)
                    sum += coeffs[0, v] * CosTable[0, x] * CosTable[v, y] * alpha;
                
                //u!=0 v==0
                for (var u = 1; u < width; u++)
                {
                    sum += coeffs[u, 0] * CosTable[u, x] * CosTable[0, y] * alpha;
                }

                //u!=0 v!=0
                for (var u = 1; u < width; u++)
                {
                    for (var v = 1; v < height; v++)
                    {
                        sum += coeffs[u, v] * CosTable[u, x] * CosTable[v, y];
                    }
                }
                output[x, y] = sum * beta;
            }
        }
    }

    private static double Beta(int height, int width)
    {
        return 1d / width + 1d / height;
    }
}