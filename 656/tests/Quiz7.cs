using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz7
    {
        public void Run()
        {
            Vector3 v = new Vector3(0.95f,-0.30f,-0.01f);
            Vector3 n = new Vector3(0.50f,0.67f,-0.55f);
            float N = 2.61f;

            v.Normalize();
            n.Normalize();

            float costheta = v % n;
            float sintheta1 = (float)Math.Sqrt(1 - costheta * costheta);
            float sintheta2 = sintheta1 / N;

            float term = (float)((((costheta * costheta) - 1) / N * N) + 1f);
            Console.WriteLine("SinTheta1: {0}\nSinTheta2: {1}", sintheta1, sintheta2);
            if (term < 0)
            {
                Console.WriteLine("There is total reflection");
                Vector3 r = -1f * v + 2f * (costheta) * n;
                Console.WriteLine("r: {0}", r);
            }
            else 
            {
                Console.WriteLine("There is refraction");
                Vector3 t = (-1f / N) * v + (float)((costheta / N) - Math.Sqrt(term)) * n;
                //t.Normalize();
                Console.WriteLine("t: {0}", t);
            }

            v = new Vector3(-0.54f, 0.83f, -0.16f);
            n = new Vector3(-0.81f, 0.57f, 0.14f);
            costheta = v % n;
            Console.WriteLine("\n\nCosTheta: {0}", costheta);
            Console.WriteLine("Theta: {0}", Math.Acos(costheta));
            Vector3 _r = -1f * v + 2f * (costheta) * n;
            Console.WriteLine("r: {0}", _r);
            Vector3 npe = -1f * v;
            Console.WriteLine("Npe: {0}", npe);
        }
    }
}
