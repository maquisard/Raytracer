using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Plane : Shape, UVInterface
    {
        public Vector3 Point { get; set; }
        public Vector3 Normal { get; set; }
        public float Sx { get; set; }  //S0
        public float Sy { get; set; } //S1
        public Vector3 Up { get; set; }

        private Vector3 P00 { get; set; }
        private Vector3 Nx { get; set; }
        private Vector3 Ny { get; set; }

        public Plane() { this.Point = Vector3.Zero; this.Normal = new Vector3(0.0f, 1.0f, 0.0f); Sx = Sy = 25; }

        public Plane(Vector3 point, Vector3 normal)
        {
            this.Point = point;
            this.Normal = normal;
            this.Normal.Normalize();
            Sx = Sy = 25;
        }

        public Plane(Vector3 normal)
        {
            this.Normal = normal;
            this.Normal.Normalize();
            this.Point = Vector3.Zero;
        }

        public override void PostLoad()
        {
            Normal.Normalize();
            Up.Normalize();
            Nx = Normal ^ Up;
            Ny = Nx ^ Normal;
            Nx.Normalize();
            Ny.Normalize();
            P00 = Point - 0.5f * ((Sx * Nx) + (Sy * Ny));
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            Vector3 p0 = Point;
            Vector3 n0 = Normal;
            float t = -1f;
            float denom = npe % n0;
            if (denom != 0) //Ray and plane are parallel
            {
                t = ((p0 - pe) % n0) / denom;
                //Now check that the plane is within the bounds of the plane
                Vector3 p = pe + t * npe;
                float px = ((p - P00) % Nx) / Sx;
                float py = ((p - P00) % Ny) / Sy;
                if (!(px <= 1f && py <= 1f))
                {
                    return -1f;
                }
            }
            return t;
        }

        public float Evaluate(Vector3 P)
        {
            Vector3 v = Point - P;
            v.Normalize();
            return Normal % v; 
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            return Normal;
        }

        public Vector2 UVCoordinates(Vector3 P)
        {
            Vector2 coordinates = new Vector2();
            float x = (Nx % (P - P00)) / Sx;
            float y = (Ny % (P - P00)) / Sy;

            x = x - (int)x;
            y = y - (int)y;
            x = x < 0 ? 1 + x : x;
            y = y < 0 ? 1 + y : y;

            coordinates.X = x;
            coordinates.Y = y;

            return coordinates;
        }
    }
}
