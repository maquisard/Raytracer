using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz5
    {
        public void Run()
        {
            float vangle = 0.7074f;
            float uangle = 0.0006f;
            float v = (float)(Math.Acos(vangle) / Math.PI);
            float sin_pi_v = (float)Math.Sin(Math.PI * v);
            float u = (float)(Math.Acos(uangle / sin_pi_v) / (2 * Math.PI));
            u = u < 0 ? (float)(2 * Math.PI) - u : u;

            float Xmax = 74;
            float Ymax = 85;

            float X = u * Xmax;
            float Y = v * Ymax;
            int I = (int)X;
            int J = (int)Y;
            float i = X - I;
            float j = Y - J;

            float Ci_j = 219;
            float Cip1_j = 237;
            float Cip1_jp1 = 217;
            float Ci_jp1 = 223;
            float C = Ci_j * (1 - j) * (1 - i) + Cip1_j * i * (1 - j) + Cip1_jp1 * i * j + Ci_jp1 * (1 - i) * j;

            Console.WriteLine("U: {0}  V: {1}", u, v);
            Console.WriteLine("X: {0}  Y: {1}", X, Y);
            Console.WriteLine("I: {0}  J: {1}", I, J);
            Console.WriteLine("i: {0}  j: {1}", i, j);
            Console.WriteLine("C: {0}\n", C);

            Vector3 p0 = new Vector3(-41, 59, -49);
            Vector3 p1 = new Vector3(-34, 70, -46);
            Vector3 p2 = new Vector3(-25, 72, -36);

            Vector3 N0 = p1 - p0;
            Vector3 N1 = p2 - p0;
            Vector3 n = N0 ^ N1;
            Console.WriteLine("Area Vector: <{0}>", n);
            n.Normalize();
            float d = n % p0;
            Console.WriteLine("Equation: ({0})X + ({1})Y + ({2})Z + ({3}) = 0", n.X, n.Y, n.Z, -d);
            Vector3 ph = new Vector3(-33.33f, 67.00f, -43.67f);
            Vector3 V = p0 - ph;
            float p = (V % N0) / N0.Norm;
            float q = (V % N1) / N1.Norm;

            if (p <= 1f && q <= 1f)
            {
                Console.WriteLine("The point is in the Triangle....");
            }
            else
            {
                Console.WriteLine("The point is not in the Triangle....");
            }
        }
    }
}
