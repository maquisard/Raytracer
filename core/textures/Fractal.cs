using edu.tamu.courses.imagesynth.core.random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class Fractal : Texture
    {
        public Vector2 Origin { get; set; }
        public Vector2 PMax { get; set; }

        public int MaxIterations { get; set; }

        private Color[] palette;

        public Fractal()
        {
            MaxIterations = 1000;
            Origin = Vector2.Zero;
            PMax = new Vector2(1f, 1f);
            palette = new Color[1500];
            
            UniformOneGenerator randomGenerator = new UniformOneGenerator();
            for (int i = 0; i < 1500; i++)
            {
                float randR = randomGenerator.Next();
                float randG = randomGenerator.Next();
                float randB = randomGenerator.Next();
                palette[i] = new Color(randR, randG, randB);
            }
        }

        public override Color ComputeColor(Vector uvcoordinates, Vector3 iPoint)
        {
            Vector2 uv = uvcoordinates as Vector2;
            float x = 10.5f * uv.X - 5.5f;
            float y = 10 * uv.Y - 5f;
            float iteration = 0;
            while ((x * x + y * y < (2 << 16)) && iteration < MaxIterations)
            {
                float xtemp = x * x - y * y + Origin.X;
                y = 2 * x * y + Origin.Y;
                x = xtemp;
                iteration++;
            }

            if (iteration < MaxIterations)
            {
                float z = (float)Math.Sqrt(x * x + y * y);
                float nu = (float)(Math.Log(Math.Log(z) / Math.Log(2)) / Math.Log(2));
                iteration = iteration + 1f - nu;
            }
            iteration = iteration < 0 ? 0 : iteration;
            int ci = (int)Math.Floor(iteration);
            Color color1 = palette[ci];
            Color color2 = palette[ci + 1];
            float t = iteration % 1;
            return new Color(color1 * t + color2 * (1 - t));
        }
    }
}
