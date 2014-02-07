using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.imaging;
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
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new Raytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            rt.Raytrace();
            Console.WriteLine("Done Raytracing....");

            //ImageData data = new ImageData(128, 128, new Color(1f, 0f, 34/255));
            //data.SaveToFile("test_image.jpg");
        }
    }
}
