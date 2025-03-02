using System.Drawing;
using System.Drawing.Imaging;

namespace JPEG.Images;

class Matrix
{
    public readonly Pixel[,] Pixels;
    public readonly int Height;
    public readonly int Width;

    public Matrix(int height, int width)
    {
        Height = height;
        Width = width;

        Pixels = new Pixel[height, width];
        for (var i = 0; i < height; ++i)
        for (var j = 0; j < width; ++j)
            Pixels[i, j] = new Pixel(0, 0, 0, PixelFormat.RGB);
    }

    public static explicit operator Matrix(Bitmap bmp)
    {
        var height = bmp.Height - bmp.Height % 8;
        var width = bmp.Width - bmp.Width % 8;
        var matrix = new Matrix(height, width);

        var bits = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bmp.PixelFormat);
        var bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
        var stride = bits.Stride;
        unsafe
        {
            var ptr = (byte*)bits.Scan0;

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var pixelIndex = (j * stride) + (i * bytesPerPixel);
                    var b = ptr[pixelIndex];
                    var g = ptr[pixelIndex + 1];
                    var r = ptr[pixelIndex + 2];

                    matrix.Pixels[j, i] = new Pixel(r, g, b, PixelFormat.RGB);
                }
            }
        }

        bmp.UnlockBits(bits);
        return matrix;
    }

    public static explicit operator Bitmap(Matrix matrix)
    {
        var height = matrix.Height;
        var width = matrix.Width;
        var bmp = new Bitmap(width, height);
        var bits = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        var stride = bits.Stride;
        unsafe
        {
            var ptr = (byte*)bits.Scan0;

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var pixel = matrix.Pixels[j, i];
                    var index = (j * stride) + (i * 3);
                    ptr[index] = ToByte(pixel.B);
                    ptr[index + 1] = ToByte(pixel.G);
                    ptr[index + 2] = ToByte(pixel.R);
                }
            }
        }
        bmp.UnlockBits(bits); 
        return bmp;
    }

    public static byte ToByte(double d)
    {
        var val = (int)d;
        return val switch
        {
            > byte.MaxValue => byte.MaxValue,
            < byte.MinValue => byte.MinValue,
            _ => (byte)val
        };
    }
}