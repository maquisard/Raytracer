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

        public float Evaluate(Vector3 p)
        {
            float r = Radius;
            return ((p - Center) % (p - Center)) - r * r;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            if (Math.Abs(npe.Norm - 1f) > 0.0001) throw new ArgumentException(String.Format("The ray vector must be a unit vector. Norm = {0}", npe.Norm));
            Vector3 pc = this.Center;
            float r = Radius;
            float b = npe % (pc - pe);
            float c = (pc - pe) % (pc - pe) - r * r;

            //Console.WriteLine("Value of c: {0}", c);
            //Console.WriteLine("Value of b: {0}", b);

            if (c < 0) throw new Exception("You are inside the sphere, readjust your camera.");

            float t = -1f; //No Intersection
            float delta = b * b - c;
            
           // Console.WriteLine("Value of Delta: {0}", delta);

            if (delta >= 0 && b >= 0)
            {
                t = b - (float)Math.Sqrt(delta);
            }
            return t;
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            Vector3 normal = p - Center;
            normal.Normalize();
            return normal;
        }
    }
}
