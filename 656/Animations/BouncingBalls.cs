using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.Animations
{
    public class BouncingBalls
    {
        private float start;
        private float end;
        private float timestep;

        public BouncingBalls(float start, float end, float timestep)
        {
            this.start = start;
            this.end = end;
            this.timestep = timestep;
        }

        public void Execute()
        {
            Console.WriteLine("Loading Scene from file...");
            Scene scene = Scene.LoadFromFile("../../data/project7/animation.scn");
            Console.WriteLine("Scene Loaded....");
            Raytracer rt = new Raytracer();
            rt.Scene = scene;
            Console.WriteLine("Starting Raytracing....");
            int frame = 1;
            String templateName = scene.Name;
            DampedVerticalFall animation = new DampedVerticalFall(0f, -9.8f, 0.8f);
            for (float t = start; t <= end; t += timestep)
            {
                scene.Name = templateName + "_" + frame;
                this.UpdateScene(t, animation, scene); 
                rt.Raytrace();
                frame++;
            }
            Console.WriteLine("Done Raytracing....");
        }

        protected void UpdateScene(float t, Animation<Sphere> animation, Scene scene)
        {
            for (int i = 0; i < scene.Shapes.Count; i++)
            {
                if (scene.Shapes[i] is Sphere && !(scene.Shapes[i] is SkySphere))
                {
                    scene.Shapes[i] = animation.Update((scene.Shapes[i] as Sphere), t);
                }
            }
        }
    }
}
