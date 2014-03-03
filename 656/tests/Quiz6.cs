using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz6
    {
        public void Run()
        {
            Triangle triangle = new Triangle();
            triangle.P0 = new Vector3(-4, 39, 44);
            triangle.P1 = new Vector3(4, 44, 55);
            triangle.P2 = new Vector3(9, 51, 65);

            Vector3[] iPoints = new Vector3[] 
            {
                new Vector3(5.8f,46.52f,58.6f),
                new Vector3(16.48f,54.9f,74.66f),
                new Vector3(-1.37f,43.55f,49.96f),
                new Vector3(8.7f,49.03f,63.15f)
            };

            foreach (Vector3 iPoint in iPoints)
            {
                triangle.UVCoordinates(iPoint);
                Console.WriteLine("U: {0}, V: {1}", triangle.U, triangle.V);
                Console.WriteLine("Is in triangle: {0}\n", triangle.IsInTriangle);
            }
        }
    }
}
