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
            Vector3 ph = new Vector3(17, 19, 22);
            Vector3 pl = new Vector3(17, 35, 34);

            Vector3 nlh = pl - ph;
            float d = nlh.Norm;
            nlh.Normalize();

            Console.WriteLine("Nlh: ({0})", nlh);
            Console.WriteLine("Distance d:={0}", d);
            Sphere sphere = new Sphere(5f, new Vector3(17f, 7f, 13f));
            this.LightSphereIntersection(sphere, ph, nlh, d);
            sphere = new Sphere(5f, new Vector3(17, 43, 40));
            this.LightSphereIntersection(sphere, ph, nlh, d);

            Color c0 = new Color(2.9f, 13.1f, 4.1f);
            Color c1 = new Color(15.2f, 13.3f, 4.9f);
            Color c2 = new Color(11.9f, 11.9f, 5.2f);

            float t = 0.5f; float s = 0.8f;
            Color c = new Color(c0 + (t * c1) + (s * c2));
            Console.WriteLine("Color c: ({0})", c);
            
            t = 0.5f; s = 0.8f;
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
