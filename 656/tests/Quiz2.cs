using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz2
    {
        //first question
        public Quiz2() { }
        public void Run()
        {
            Console.WriteLine("First Question");
            Vector3 pe = new Vector3(19, 8, 11);
            Vector3 npe = new Vector3(-7 ,-4, -4);
            npe.Normalize();
            Plane plane = new Plane(new Vector3(12, 0, 0), new Vector3(1, 0, 0));
            float t = plane.Intersect(pe, npe);
            if (t > 0)
            {
                Vector3 pIntersection = pe + npe * t;
                Vector3 iNormal = plane.NormalAt(pIntersection);
                Console.WriteLine("Intersect Plane At: <{0}>", pIntersection);
                Console.WriteLine("Normal at Intersection: <{0}>", iNormal);

                PointLight light = new PointLight();
                light.Position = new Vector3(14, 6, 8);
                Vector3 lightVector = light.Position - pIntersection;
                lightVector.Normalize();
                float angle = lightVector % iNormal;
                Console.WriteLine("Cos of Angle betweeen Normal and Light Vector: {0}", angle);

                Vector3 v = -1f * npe;
                Vector3 r = 2f * (iNormal % v) * iNormal - v;
                r.Normalize();
                angle = r % lightVector;
                Console.WriteLine("Cos of Angle between Eye Reflection and Light Vector: {0}", angle);
            }

            Console.WriteLine("\nSecond Question: ");
            pe = new Vector3(17, 13, 15);
            Sphere sphere = new Sphere(18, new Vector3(23,19,32));
            float value = sphere.Evaluate(pe);
            Console.WriteLine("Value of Sphere Evaluation: {0}", value);
            Console.WriteLine("Does your program have to stop without starting ray tracing? {0}", value < 0 ? "Yes" : "No");

            Console.WriteLine("\nThird Question: ");
            pe = new Vector3(8, 19, 18);
            sphere = new Sphere(18.4f, new Vector3(14, 25, 35));
            npe = new Vector3(6, 6, 17);
            npe.Normalize();
            t = sphere.Intersect(pe, npe);
            if (t > 0)
            {
                Console.WriteLine("Intersection at t: {0}", t);
                Vector3 pIntersection = pe + npe * t;
                Vector3 iNormal = sphere.NormalAt(pIntersection);
                Console.WriteLine("Intersection at Point p: <{0}>", pIntersection);
                Console.WriteLine("Normal at Intersection: <{0}>", iNormal);
            }
            else
            {
                Console.WriteLine("There is no intersection.");
            }

        }
    }
}
