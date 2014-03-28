using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class SkySphere : Sphere
    {
        public SkySphere() { }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            if (Math.Abs(npe.Norm - 1) > 0.01) throw new ArgumentException("The ray vector must be a unit vector.");
            Vector3 pc = this.Center;
            float r = Radius;
            float b = npe % (pc - pe);
            float c = (pc - pe) % (pc - pe) - r * r;

            float t = -1f; //No Intersection
            float delta = b * b - c;
            if (delta >= 0)
            {
                t = b + (float)Math.Sqrt(delta);
            }
            return t;
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            Vector3 normal = Center - p;
            normal.Normalize();
            return normal;
        }
    }
}
