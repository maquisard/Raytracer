using edu.tamu.courses.imagesynth.Animations.Custom;
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

            //Console.WriteLine("Loading Scene from file...");
            var timer = System.Diagnostics.Stopwatch.StartNew();
            Scene scene = Scene.LoadFromFile("../../data/project2.scn");
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new Raytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            rt.Raytrace();
            Console.WriteLine("Done Raytracing....");
            timer.Stop();
            Console.WriteLine("Duration: {0}", timer.Elapsed.ToString("mm' : 'ss"));
            //Project2_LightMotion animation = new Project2_LightMotion(-20f, 10f);
            //animation.Execute();

        }
    }
}
