using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    class Program
    {
        public static void Main(string[] args)
        {

            Quiz3 quiz = new Quiz3();
            quiz.Run();

            //Scene scene = Scene.LoadFromFile("../../data/testjson.scn");
            //Raytracer rt = new Raytracer();
            //rt.Scene = scene;
            //rt.Compute(255, 190, 1, 3, 4, 5, 0.0f, 0.2f);

            //IntersectionTest test = new IntersectionTest();
            //test.run();
            
            //float sx = 8f;
            //float sy = 12f;
            //float d = 10;
            //Vector3 view = new Vector3(0f, 6f, 8f);
            //Vector3 pe = -1f * view;
            //Vector3 up = new Vector3(1f, 0f, 0f);
            //Vector3 n2 = -1f * (view / view.Norm);
            //Vector3 no = (view ^ up);
            //no.Normalize();
            //Vector3 n1 = (no ^ view) / view.Norm;
            //Vector3 pc = pe - d * n2;
            //Vector3 p0 = pc - 0.5f * ((no * sx) + (n1 * sy));

            //float X = 235.700f;
            //float Y = 105.520f;
            //float Xmax = 800f;
            //float Ymax = 1200f;
            //float x = X / Xmax; float y = Y / Ymax;

            //Vector3 pp = p0 + (x * sx * no) + (y * sy * n1);
            //Vector3 npe = pp - pe;
            //npe.Normalize();
            //Console.WriteLine(n2.ToString());
            //Console.WriteLine(no.ToString());
            //Console.WriteLine(n1.ToString());
            //Console.WriteLine(pc.ToString());
            //Console.WriteLine(p0.ToString());
            //Console.WriteLine(pp.ToString());
            //Console.WriteLine(npe.ToString());
        }
    }
}
