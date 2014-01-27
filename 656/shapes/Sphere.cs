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
            throw new NotImplementedException();
        }
    }
}
