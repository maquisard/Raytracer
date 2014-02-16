using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz4
    {
        public void Run()
        {
            Ellipsoid e = new Ellipsoid();
            e.S0 = 9f;
            e.S1 = 4f;
            e.S2 = 3f;
            e.Up = new Vector3(1f, 0f, 0f);
            e.Direction = new Vector3(4f, 6f, 12f);
            e.Center = new Vector3(11f, 2f, 4f);
            e.PostLoad();

            Console.WriteLine("Intersect At {0}", e.Intersect(null, null));

            Console.WriteLine("N0 : {0}\nN1 : {1}\nN2 : {2}\n", e.N0, e.N1, e.N2);
            Vector3 p = new Vector3(11f, 10.01f, -0.0499999f);
            Vector3 Normal = e.NormalAt(p);
            Console.WriteLine("Normal : {0}", Normal);
        }
    }
}
