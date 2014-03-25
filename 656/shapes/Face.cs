using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Face : Shape, UVInterface
    {
        public Vertex V0 { get; set; }
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public Vector2 ST { get; set; }

        public override Vector3 NormalAt(Vector3 p)
        {
            float s = ST.X;
            float t = ST.Y;
            Vector3 normal = (1 - s - t) * V0.Normal + s * V1.Normal + t * V2.Normal;
            return normal;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            Vector3 P0 = V0.Point;
            Vector3 P1 = V1.Point;
            Vector3 P2 = V2.Point;

            Vector3 N = (P1 - P0) ^ (P2 - P0);
            N.Normalize();
            Plane plane = new Plane(P0, N);
            plane.Up = P2 - P0;
            plane.PostLoad();
            float t = plane.Intersect(pe, npe);
            if (t < 0) return t;

            Vector3 p = pe + t * npe;
            ST = ComputeST(p);

            if(ST == null) return -1f; // the point is not in the triangle

            return t;
        }

        public Vector2 UVCoordinates(Vector3 p)
        {
            float s = ST.X;
            float t = ST.Y;

            float Us = (1 - s - t) * V0.UV.X + s * V1.UV.X + t * V2.UV.X;
            float Vs = (1 - s - t) * V0.UV.Y + s * V1.UV.Y + t * V2.UV.Y;

            return new Vector2(Us, Vs);
        }

        private Vector2 ComputeST(Vector3 p)
        {
            Vector3 P0 = V0.Point;
            Vector3 P1 = V1.Point;
            Vector3 P2 = V2.Point;

            Vector3 A = (P1 - P0) ^ (P2 - P0);
            Vector3 A0 = (p - P1) ^ (p - P2);
            Vector3 A1 = (p - P2) ^ (p - P0);
            Vector3 A2 = (p - P0) ^ (p - P1);

            float U = A.X <= 0.01 ? (A.Y <= 0.01 ? A1.Z / A.Z : A1.Y / A.Y) : A1.X / A.X;
            float V = A.X <= 0.01 ? (A.Y <= 0.01 ? A2.Z / A.Z : A2.Y / A.Y) : A2.X / A.X;
            bool IsInTriangle = ((U > 0f && U <= 1f) && (V > 0f && V <= 1f) && ((1f - U - V) > 0f && (1f - U - V) <= 1f));

            if (!IsInTriangle) return null;

            return new Vector2(U, V);
        }
    }
}
