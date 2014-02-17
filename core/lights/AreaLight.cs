using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class AreaLight : Light
    {
        public Vector3 Vup { get; set; }
        public Vector3 Nl { get; set; }
        public float Sx { get; set; }
        public float Sy { get; set; }

        public Vector3 N0 { get; set; }
        public Vector3 N1 { get; set; }

        public float OffsetX { get; set; }
        public float OffsetY { get; set; }

        private Light[,] grid;

        public override void PostLoad()
        {
            this.Update();
        }

        public override Color ComputeFinalLightColor(Vector3 ph)
        {
            return this.Color;
        }

        public void Update()
        {
            N0 = Nl ^ Vup;
            N1 = N0 ^ Nl;
            Vup.Normalize();
            Nl.Normalize();
            N0.Normalize();
            N1.Normalize();
        }

        public void CreateLightGrid(int m, int n)
        {
            grid = new PointLight[m, n];
            float nx = Sx / (float)m;
            float ny = Sy / (float)n;
            Vector3 currentPoint = Position - 0.5f * ((Sx * N0) + (Sy * N1));
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    currentPoint = currentPoint + (0.5f * (float)i * nx) * N0 + (0.5f * (float)j * ny) * N1;
                    grid[i, j] = new PointLight();
                    grid[i, j].Position = currentPoint;
                    grid[i, j].Color = Color;
                }
            }
        }

        public Light GetSubLight(int m, int n, int i, int j, float r)
        {
            float k = (float)(i + j * m);
            float ks = (k + r) % ((float)(m * n));
            int J = (int)(ks / (float)m);
            int I = (int)(ks - (float)J);
            return grid[I, J];
        }

    }
}
