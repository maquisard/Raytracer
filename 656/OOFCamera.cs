using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class OOFCamera : Camera
    {
        public float Ax { get; set; }
        public float Ay { get; set; }

        private float M { get; set; }
        private float N { get; set; }

        private float Nx { get; set; }
        private float Ny { get; set; }
        private Vector3 Origin { get; set; }

        public int Ith { get; set; }
        public int Jth { get; set; }

        public float RndOffsetX { get; set; }
        public float RndOffsetY { get; set; }

        public void CreateLightGrid(int m, int n)
        {
            M = (float)m;
            N = (float)n;
            Nx = Ax / M;
            Ny = Ay / N;
            Origin = this.Pe - 0.5f * ((Ax * N0) + (Ay * N1));
        }

        public Vector3 ComputePe()
        {
            float s = Ith * Nx + (0.25f * RndOffsetX / M);
            float t = Jth * Ny + (0.25f * RndOffsetY / N);

            if (s > Ax || t > Ay)
            {
                throw new Exception("Fuck??");
            }
            return Origin + (s * N0 + t * N1);
        }
    }
}
