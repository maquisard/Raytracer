using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.Animations.Custom
{
    public class Project2_LightMotion
    {
        private float start;
        private float end;

        public Project2_LightMotion(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        public void Execute()
        {
            Console.WriteLine("Loading Scene from file...");
            Scene scene = Scene.LoadFromFile("../../data/project2.scn");
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new Raytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            int frame = 1;
            for (float t = start; t <= end; t += 0.2f)
            {
                scene.Lights[0].Position.X = t;
                scene.Name += "" + frame;
                rt.Raytrace();
                frame++;
            }
            Console.WriteLine("Done Raytracing....");
        }
    }
}
