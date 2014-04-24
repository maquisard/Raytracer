using edu.tamu.courses.imagesynth.Animations;
using edu.tamu.courses.imagesynth.Animations.Custom;
using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.imaging;
using edu.tamu.courses.imagesynth.core.textures;
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

            //Quiz9 quiz = new Quiz9();
            //quiz.Run();

            LaunchRaytracer();
            //RunAnimation();
            //WallPaper wallpaper = new WallPaper("../../data/textures/wallpapertest-2-small.png");
            //wallpaper.CountX = 20;
            //wallpaper.CountY = 20;
            //wallpaper.Generate("../../data/textures/wallpaper-2-small.png");
            //Console.WriteLine("Done....");
        }

        public static void RunAnimation()
        {
            BouncingBalls animation = new BouncingBalls(0f, 1f, 0.01f);
            animation.Execute();
        }

        public static void LaunchRaytracer()
        {
            Console.WriteLine("Loading Scene from file...");
            var timer = System.Diagnostics.Stopwatch.StartNew();
            Scene scene = Scene.LoadFromFile("../../data/testjson.scn");
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new GIRaytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            rt.Raytrace();
            Console.WriteLine("Done Raytracing....");
            timer.Stop();
            Console.WriteLine("Duration: {0}", timer.Elapsed.ToString("mm' : 'ss"));
        }
    }
}
