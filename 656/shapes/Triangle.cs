using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Triangle : Shape
    {
        public Vector3 P0 { get; set; }
        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Vector3 Normal { get; set; }

        public float U { get; set; }
        public float V { get; set; }
        public bool IsInTriangle { get; set; }

        public override Vector3 NormalAt(Vector3 p)
        {
            return Normal;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            Vector3 N = (P1 - P0) ^ (P2 - P0);
            N.Normalize();
            Plane plane = new Plane(P0, N);
            float t = plane.Intersect(pe, npe);
            if (t < 0) return t;
            Vector3 p = pe + t * npe;
            UVCoordinates(p);
            return -1f;
        }

        public Vector2 UVCoordinates(Vector3 p)
        {
            IsInTriangle = false;
            Vector3 A = (P1 - P0) ^ (P2 - P0);
            Vector3 A0 = (p - P1) ^ (p - P2);
            Vector3 A1 = (p - P2) ^ (p - P0);
            Vector3 A2 = (p - P0) ^ (p - P1);

            U = A.X <= 0.01 ? (A.Y <= 0.01 ? A1.Z / A.Z : A1.Y / A.Y) : A1.X / A.X;
            V = A.X <= 0.01 ? (A.Y <= 0.01 ? A2.Z / A.Z : A2.Y / A.Y) : A2.X / A.X;
            IsInTriangle = ((U > 0f && U <= 1f) && (V > 0f && V <= 1f) && ((1f - U - V) > 0f && (1f - U - V) <= 1f));

            return new Vector2(U, V);
        }
    }
}
