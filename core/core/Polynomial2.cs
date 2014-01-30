using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core
{
    public class Polynomial2
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }

        public Polynomial2(float a, float b, float c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        public float[] FindRoots()
        {
            float discriminant = B * B - 4f * A * C;
            if (discriminant < 0)
            {
                return new float[0];
            }
            else if (discriminant == 0)
            {
                return new float[] { -B / (2f * A) };
            }
            else
            {
                float sqrt = (float)Math.Sqrt(discriminant);
                return new float[] { (-B - sqrt) / (2f * A), (-B + sqrt) / (2f * A) };
            }
        }
    }
}
