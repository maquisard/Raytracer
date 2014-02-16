using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.imaging;
using edu.tamu.courses.imagesynth.shapes;
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

            //Quiz3 quiz = new Quiz3();
            //quiz.Run();

            Console.WriteLine("Loading Scene from file...");
            Scene scene = Scene.LoadFromFile("../../data/testjson.scn");
            //Cylinder cylinder = new Cylinder();
            //cylinder.Center = new Vector3(-3f, 0f, 0f);
            //cylinder.Direction = new Vector3(0f, -1f, 0f);
            //cylinder.Up = new Vector3(-1f, 0f, 0f);
            //cylinder.S0 = 2f;
            //cylinder.S1 = 2f;
            //cylinder.S2 = 1f;
            //cylinder.PostLoad();
            //scene.Shapes.Add(cylinder);
            
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new Raytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            rt.Raytrace();
            Console.WriteLine("Done Raytracing....");

        }
    }
}
