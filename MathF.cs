using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine
{
    public static class MathF
    {
        public const float E = (float)Math.E;
        public const float PI = (float)Math.PI;

        #region Sin, cos, tan
        public static float Sin(float value)
        {
            return (float)Math.Sin(value);
        }
        public static float Cos(float value)
        {
            return (float)Math.Cos(value);
        }
        public static float Tan(float value)
        {
            return (float)Math.Tan(value);
        }
        public static float Sinh(float value)
        {
            return (float)Math.Sinh(value);
        }
        public static float Cosh(float value)
        {
            return (float)Math.Cosh(value);
        }
        public static float Tanh(float value)
        {
            return (float)Math.Tanh(value);
        }
        public static float Asin(float value)
        {
            return (float)Math.Asin(value);
        }
        public static float Acos(float value)
        {
            return (float)Math.Acos(value);
        }
        public static float Atan(float value)
        {
            return (float)Math.Atan(value);
        }
        public static float Atan2(float x, float y)
        {
            return (float)Math.Atan2(x, y);
        }
        #endregion

        
        public static float Ceiling(float value)
        {
            return (float)Math.Ceiling(value);
        }
        public static float Floor(float value)
        {
            return (float)Math.Floor(value);
        }

        public static float Exp(float value)
        {
            return (float)Math.Exp(value);
        }

        public static float IEEERemainder(float x, float y)
        {
            return (float)Math.IEEERemainder(x, y);
        }

        #region Log
        public static float Log(float value)
        {
            return (float)Math.Log(value);
        }
        public static float Log(float value, float newBase)
        {
            return (float)Math.Log(value, newBase);
        }
        public static float Log10(float value)
        {
            return (float)Math.Log10(value);
        }
        #endregion

        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        public static float Round(float value)
        {
            return (float)Math.Round(value);
        }
    }
}
