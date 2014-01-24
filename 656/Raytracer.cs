using edu.tamu.courses.imagesynth.core;
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
                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
                    {
                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
                        {

                        }
                    }
                }
            }
        }
    }
}
