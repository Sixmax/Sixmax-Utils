using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXUtils.Extensions
{
    public static class NumericExtensions
    {
        public static int Clamp(this int A, int B, int C) => Math.Min(Math.Max(A, B), C);
        public static float Clamp(this float A, float B, float C) => Math.Min(Math.Max(A, B), C);
        public static double Clamp(this double A, double B, double C) => Math.Min(Math.Max(A, B), C);
        public static byte Clamp(this byte A, byte B, byte C) => Math.Min(Math.Max(A, B), C);
        public static decimal Clamp(this decimal A, decimal B, decimal C) => Math.Min(Math.Max(A, B), C);
    }
}
