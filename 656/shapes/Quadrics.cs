using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public abstract class Quadrics : Shape
    {
        protected abstract float A00 { get; }
        protected abstract float A02 { get; }
        protected abstract float A12 { get; }
        protected abstract float A22 { get; }
        protected abstract float A21 { get; }

        public float S0 { get; set; }
        public float S1 { get; set; }
        public float S2 { get; set; }

        public Vector3 N0 { get; set; }
        public Vector3 N1 { get; set; }
        public Vector3 N2 { get; set; }

        public Vector3 Direction { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Center { get; set; }

        public override void PostLoad()
        {
            this.N2 = Direction;
            N2.Normalize();
            Up.Normalize();
            N0 = N2 ^ Up;
            N1 = N0 ^ N2;
            N0.Normalize();
            N1.Normalize();
        }

        public float Evaluate(Vector3 P)
        {
            Vector3 v = P - Center; //P-Pc
            return A02 * GSquared(N0, v, S0) + A02 * GSquared(N0, v, S0) + A12 * GSquared(N1, v, S1) + A22 * GSquared(N2, v, S2) + A21 * G(N2, v, S2) + A00;
        }

        private float G(Vector3 Ni, Vector3 p_pc, float Si)
        {
            return (Ni % p_pc) / Si;
        }

        private float GSquared(Vector3 Ni, Vector3 p_pc, float Si)
        {
            float value = (Ni % p_pc) / Si;
            return value * value;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            float A = A02 * GSquared(N0, npe, S0) + A12 * GSquared(N1, npe, S1) + A22 * GSquared(N2, npe, S2);
            float[] a = { A02, A12, A22 };
            float[] s = { S0, S1, S2 };
            Vector3[] n = { N0, N1, N2 };
            Vector3 v = pe - Center;
            float B = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                B += a[i] * 2f * ((n[i] % npe) * (n[i] % v)) / (s[i] * s[i]);
            }
            B += A21 * (N2 % npe) / S2;

            float C = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                C += a[i] * GSquared(n[i], v, s[i]);
            }
            C += A21 * G(N2, v, S2) + A00;


            float delta = B * B - 4f * A * C;
            if (delta < 0f) return -1f;

            float t1 = (-B + (float)Math.Sqrt(delta)) / (2f * A);
            float t2 = (-B - (float)Math.Sqrt(delta)) / (2f * A);

            return Math.Min(t1, t2);
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            float[] a = { A02, A12, A22 };
            float[] s = { S0, S1, S2 };
            Vector3[] n = { N0, N1, N2 };
            Vector3 v = p - Center;
            Vector3 gradient = Vector3.Zero;
            for (int i = 0; i < a.Length; i++)
            {
                gradient = gradient + ((2f * a[i] * (n[i] % v)) / (s[i] * s[i])) * n[i];
            }
            gradient = gradient + (A21 / S2) * N2;
            gradient.Normalize();
            return gradient;
        }
    }
}
