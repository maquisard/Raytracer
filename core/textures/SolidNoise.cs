using edu.tamu.courses.imagesynth.core.random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class SolidNoise
    {
        private Vector3[] grad = new Vector3[16];
        private int[] phi = new int[16];

        public SolidNoise()
        {
            int i;
            UniformOneGenerator random = new UniformOneGenerator();

            grad[0] = new Vector3( 1,  1, 0);
            grad[1] = new Vector3(-1,  1, 0);
            grad[2] = new Vector3( 1, -1, 0);
            grad[3] = new Vector3(-1, -1, 0);

            grad[4] = new Vector3( 1, 0,  1);
            grad[5] = new Vector3(-1, 0,  0);
            grad[6] = new Vector3( 1, 0, -1);
            grad[7] = new Vector3(-1, 0, -1);

            grad[8]  = new Vector3(0,  1,  1);
            grad[9]  = new Vector3(0, -1,  0);
            grad[10] = new Vector3(0,  1, -1);
            grad[11] = new Vector3(0, -1, -1);

            grad[12] = new Vector3( 1, 1,  0);
            grad[13] = new Vector3(-1, 1,  0);
            grad[14] = new Vector3(0, -1,  1);
            grad[15] = new Vector3(0, -1, -1);

            for (i = 0; i < 16; i++)
            {
                phi[i] = i;
            }

            for (i = 14; i >= 0; i--)
            {
                int target = (int)(random.Next() * i);
                int temp = phi[i + 1];
                phi[i + 1] = phi[target];
                phi[target] = temp;
            }
        }

        public float Turbulence(Vector3 p, int depth)
        {
            float sum = 0.0f;
            float weight = 1.0f;
            Vector3 temp = new Vector3(p.X, p.Y, p.Z);
            sum = Math.Abs(Noise(temp));

            for (int i = 0; i < depth; i++)
            {
                weight = weight * 2;
                temp = p * weight;
                sum += Math.Abs(Noise(temp)) / weight;
            }

            return sum;
        }

        public float DTurbulence(Vector3 p, int depth, float d)
        {
            float sum = 0.0f;
            float weight = 1.0f;
            Vector3 temp = new Vector3(p.X, p.Y, p.Z);
            sum = Math.Abs(Noise(temp)) / d;

            for (int i = 0; i < depth; i++)
            {
                weight = weight * d;
                temp = p * weight;
                sum += Math.Abs(Noise(temp)) / d;
            }

            return sum;
        }

        public float Noise(Vector3 p)
        {
            int fi, fj, fk;
            float fu, fv, fw, sum;
            Vector3 v;

            fi = (int)Math.Floor(p.X);
            fj = (int)Math.Floor(p.Y);
            fk = (int)Math.Floor(p.Z);
            fu = p.X - (float)fi;
            fv = p.Y - (float)fj;
            fw = p.Z - (float)fk;
            sum = 0;

            v = new Vector3(fu, fv, fw);
            sum += Knot(fi, fj, fk, v);

            v = new Vector3(fu - 1, fv, fw);
            sum += Knot(fi + 1, fj, fk, v);

            v = new Vector3(fu, fv - 1, fw);
            sum += Knot(fi, fj + 1, fk, v);

            v = new Vector3(fu, fv, fw - 1);
            sum += Knot(fi, fj, fk + 1, v);

            v = new Vector3(fu - 1, fv - 1, fw);
            sum += Knot(fi + 1, fj + 1, fk, v);

            v = new Vector3(fu - 1, fv, fw - 1);
            sum += Knot(fi + 1, fj, fk + 1, v);

            v = new Vector3(fu, fv - 1, fw - 1);
            sum += Knot(fi, fj + 1, fk + 1, v);

            v = new Vector3(fu - 1, fv - 1, fw - 1);
            sum += Knot(fi + 1, fj + 1, fk + 1, v);

            return sum;
        }

        private float Omega(float t)
        {
            t = (t > 0.0f) ? t : -t;
            return (-6.0f * t * t * t * t * t + 15.0f * t * t * t * t - 10.0f * t * t * t + 1.0f);
        }

        private Vector3 Gamma(int i, int j, int k)
        {
            int idx = phi[Math.Abs(k) % 16];
            idx = phi[Math.Abs(j + idx) % 16];
            idx = phi[Math.Abs(i + idx) % 16];
            return grad[idx];
        }

        private float Knot(int i, int j, int k, Vector3 v)
        {
            return (Omega(v.X) * Omega(v.Y) * Omega(v.Z)) * (Gamma(i, j, k) % v);
        }

        private int IntGamma(int i, int j)
        {
            int idx = phi[Math.Abs(j) % 16];
            idx = phi[Math.Abs(i + idx) % 16];
            return idx;
        }
    }
}
