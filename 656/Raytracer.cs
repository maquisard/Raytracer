using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.random;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class Raytracer
    {
        public Scene Scene;
        public float X { get; private set; }
        public float Y { get; private set; }
        public Vector3 Pp { get; private set; }
        public Vector3 Npe { get; private set; }

        private UniformOneGenerator randomGenerator = new UniformOneGenerator();

        public void Compute(int I, int J, int i, int j, int m, int n, float rx, float ry)
        {
            X = I + (i / (float)m) + (rx / (float)m);
            Y = J + (j / (float)n) + (ry / (float)n);
            float x = X / Scene.Camera.Xmax;
            float y = Y / Scene.Camera.Ymax;
            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
            Npe = Pp - Scene.Camera.Pe;
            Npe.Normalize();
            Console.WriteLine("\nX: {0}\nY: {1}\nPP: {2}\nNPE: {3}", X, Y, Pp, Npe);
        }

        public void Raytrace()
        {
            for (int I = 0; I < Scene.Camera.Xmax; I++)
            {
                for (int J = 0; J < Scene.Camera.Ymax; J++)
                {
                    Color color = Color.BLACK;
                    float randx = randomGenerator.Next() / (float)Scene.MSamplePerPixels;
                    float randy = randomGenerator.Next() / (float)Scene.NSamplePerPixels;

                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
                    {
                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
                        {
                            X = I + (i / (float)Scene.MSamplePerPixels) + randx;
                            Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
                            float x = X / Scene.Camera.Xmax;
                            float y = Y / Scene.Camera.Ymax;
                            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
                            Npe = Pp - Scene.Camera.Pe;
                            Npe.Normalize();
                            Shape shape = Scene.GetIntersectedShape(Scene.Camera.Pe, Npe); //Implement the Get Intersected Shape
                            if (shape == null)
                            {
                                //return the color Black at X, Y
                            }
                            else 
                            { 
                                //get the shader, get the scene lights and compute the color at X, Y 
                            }
                        }
                    }
                }
            }
        }
    }
}
