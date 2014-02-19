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


        private float M { get; set; }
        private float N { get; set; }

        private float Nx { get; set; }
        private float Ny { get; set; }
        private Vector3 Origin { get; set; }

        public int Rnd { get; set; }
        public int Ith { get; set; }
        public int Jth { get; set; }

        public float RndOffsetX { get; set; }
        public float RndOffsetY { get; set; }


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
            M = (float)m;
            N = (float)n;
            Nx = Sx / M;
            Ny = Sy / N;
            Origin = Position - 0.5f * ((Sx * N0) + (Sy * N1));
        }

        public override Vector3 ComputeLightVector(Vector3 P)
        {
            int k = (int)(Ith + Jth * N);
            int ks = (int)((k + Rnd) % (M * N));
            float J = (ks / M);
            float I = (ks - J) / N;

            float s = Ith * Nx + (RndOffsetX / M);
            float t = Jth * Ny + (RndOffsetY / N);

            //s = s / Sx;
            //t = t / Sy;

            if (s > Sx || t > Sy)
            {
                throw new Exception("Fuck??");
            }

            Position = Origin + (s * N0  + t * N1);
            return base.ComputeLightVector(P);
        }
    }
}
