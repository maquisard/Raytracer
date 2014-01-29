using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Sphere : Shape
    {
        public float Radius { get; set; }
        public Vector3 Center { get; set; }

        public Sphere() { this.Radius = 1.0f; this.Center = Vector3.Zero; }

        public Sphere(float radius, Vector3 center)
        {
            this.Radius = radius;
            this.Center = center;
        }

        public Sphere(float radius)
        {
            this.Radius = radius;
            this.Center = Vector3.Zero;
        }


        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            if (npe.Norm != 1) throw new ArgumentException("The ray vector must be a unit vector.");
            Vector3 pc = this.Center;
            float r = Radius;
            float b = npe % (pc - pe);
            float c = (pc - pe) % (pc - pe) - r * r;
            if (c < 0) throw new Exception("You are inside the sphere, readjust your camera.");

            float t = -1f; //No Intersection
            float delta = b * b - c;
            if (delta >= 0 && b >= 0)
            {
                t = b - (float)Math.Sqrt(delta);
            }
            return t;
        }
    }
}
