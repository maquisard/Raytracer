using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class Math
    {
        public static float[] Zero3
        { get { return new float[] { 0f, 0f, 0f }; } }

        #region addition operations
        public static float[] Add(float[] a, float[] b)
        {
            CheckIntegrity(a, b);
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }

        public static float[] Add(float a, float[] b)
        {
            float[] c = new float[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a + b[i];
            }
            return c;
        }
        #endregion

        #region substraction operations
        public static float[] Substract(float[] a, float[] b)
        {
            CheckIntegrity(a, b);
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a[i] - b[i];
            }
            return c;
        }

        public static float[] Substract(float a, float[] b)
        {
            float[] c = new float[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a - b[i];
            }
            return c;
        }
        #endregion

        #region multiplication operations
        public static float[] Multiply(float[] a, float[] b)
        {
            CheckIntegrity(a, b);
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a[i] * b[i];
            }
            return c;
        }
        public static float[] Multiply(float a, float[] b)
        {
            float[] c = new float[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a * b[i];
            }
            return c;
        }
        #endregion

        #region division operations
        public static float[] Divide(float[] a, float[] b)
        {
            CheckIntegrity(a, b);
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a[i] / b[i];
            }
            return c;
        }
        public static float[] Divide(float a, float[] b)
        {
            float[] c = new float[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = a / b[i];
            }
            return c;
        }
        #endregion

        public static float Dot(float[] a, float[] b)
        {
            CheckIntegrity(a, b);
            float dotProduct = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                dotProduct += a[i] * b[i];
            }
            return dotProduct;
        }


        public static void CheckIntegrity(float[] a, float[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("the arguments must have the same length.");
        }
    }
}
