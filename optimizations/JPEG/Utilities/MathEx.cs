using System;

namespace JPEG.Utilities
{
    public static class MathEx
    {
        public static double Sum(ushort from, ushort to, Func<ushort, double> function)
        {
            var sum = 0.0;
            for (var i = from; i < to; i++)
            {
                sum += function(i);
            }
            return sum;
        }

        public static double SumByTwoVariables(ushort from1, ushort to1, ushort from2, ushort to2, Func<ushort, ushort, double> function)
        {
            var sum = 0.0;
            for (var x = from1; x < to1; x++)
            {
                for (var y = from2; y < to2; y++)
                {
                    sum += function(x, y);
                }
            }
            return sum;
        }

        public static void LoopByTwoVariables(ushort from1, ushort to1, ushort from2, ushort to2, Action<ushort, ushort> function)
        {
            for (var x = from1; x < to1; x++)
            {
                for (var y = from2; y < to2; y++)
                {
                    function(x, y);
                }
            }
        }
    }
}