using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz3
    {
        public Quiz3() { }

        public void Run()
        {
            Vector3 ph = new Vector3(22, 36, 24);
            Vector3 pl = new Vector3(26, 44, 32);

            Vector3 nlh = pl - ph;
            float d = nlh.Norm;
            nlh.Normalize();

            Console.WriteLine("Nlh: ({0})", nlh);
            Console.WriteLine("Distance d:={0}", d);
            Sphere sphere = new Sphere(6f, new Vector3(14f, 20f, 8f));
            this.LightSphereIntersection(sphere, ph, nlh, d);
            sphere = new Sphere(6f, new Vector3(30, 52, 40));
            this.LightSphereIntersection(sphere, ph, nlh, d);

            Color c0 = new Color(0.3f, 7.2f, 19f);
            Color c1 = new Color(4.5f, 13f, 9.3f);
            Color c2 = new Color(1.8f, 4.3f, 2.8f);

            float t = 0.4f; float s = 0.9f;
            Color c = new Color(c0 + (t * c1) + (s * c2));
            Console.WriteLine("Color c: ({0})", c);
            
            t = 0.4f; s = 0.9f;
            c = new Color((1 - s) * ((1 - t) * c0 + t * c1) + s * c2);
            Console.WriteLine("Color c: ({0})", c);
        }

        public void LightSphereIntersection(Sphere sphere, Vector3 ph, Vector3 nlh, float d)
        {
            float t = sphere.Intersect(ph, nlh);
            Console.WriteLine("Intersection at t: {0}", t);

            if (t > 0 && t <= d)
            {
                Console.WriteLine("Shadow is casted.");
            }
            else
            {
                Console.WriteLine("Shadow is not casted.");
            }
        }
    }
}
