using System;
using System.Threading.Tasks;
using JPEG.Utilities;

namespace JPEG;

public class DCT
{
	public static double[,] DCT2D(double[,] input)
	{
		var height = input.GetLength(0);
		var width = input.GetLength(1);
		var coeffs = new double[width, height];
		var beta = Beta(height, width);

		Parallel.For(0, width, u =>
		{
			for (var v = 0; v < height; v++)
			{

				var alphaU = Alpha(u);
				var alphaV = Alpha(v);
				var alphaBeta = alphaU * alphaV * beta;
				var sum = 0.0;

				for (var x = 0; x < width; x++)
				{
					for (var y = 0; y < height; y++)
					{
						sum += BasisFunction(input[x, y], u, v, x, y, height, width);
					}
				}
				coeffs[u, v] = sum * alphaBeta;
			}
		});
		return coeffs;
	}

	public static void IDCT2D(double[,] coeffs, double[,] output)
	{
		var height = coeffs.GetLength(0);
		var width = coeffs.GetLength(1);
		var beta = Beta(height, width);
		Parallel.For(0, width, x =>
		{
			for (var y = 0; y < height; y++)
			{
				var sum = 0.0;

				for (var u = 0; u < width; u++)
				{
					for (var v = 0; v < height; v++)
					{
						sum += BasisFunction(coeffs[u, v], u, v, x, y, height, width) *
						       Alpha(u) * Alpha(v);
					}
				}
				output[x, y] = sum * beta;
			}
		});
	}

	public static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
	{
		var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
		var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

		return a * b * c;
	}

	private static double Alpha(int u)
	{
		if (u == 0)
			return 1 / Math.Sqrt(2);
		return 1;
	}

	private static double Beta(int height, int width)
	{
		return 1d / (width + height);
	}
}