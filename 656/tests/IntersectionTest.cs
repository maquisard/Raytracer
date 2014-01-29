using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class IntersectionTest
    {
        public void run()
        {
            Dictionary<String, Shape> shapes = new Dictionary<String, Shape>();
            shapes["back_plane"] = new Plane(new Vector3(-2f, 0f, 0f), new Vector3(1f, 0f, 0f));
            shapes["big_sphere"] = new Sphere(2f);
            shapes["small_sphere"] = new Sphere(0.5f, new Vector3(2.5f, 0f, 0f));
            shapes["medium_sphere"] = new Sphere(1f, new Vector3(0f, -4f, 0f));
            
            Vector3 pe = new Vector3(4f, 0f, 0f);
            List<Vector3> endPoints = new List<Vector3>();
            endPoints.Add(new Vector3(4f, 2f, 0f));
            endPoints.Add(new Vector3(0f, 4f, 0f));
            endPoints.Add(new Vector3(0f, 2f, 0f));
            endPoints.Add(new Vector3(0f, 0.25f, 0f));
            endPoints.Add(new Vector3(0f, -4f, 0f));
            endPoints.Add(new Vector3(0f, -6f, 0f));
            endPoints.Add(new Vector3(5f, 5f, 0f));
            endPoints.Add(new Vector3(5f, -5f, 0f));

            foreach (Vector3 endPoint in endPoints)
            {
                Vector3 npe = (endPoint - pe);
                npe.Normalize();
                this.ComputeInterSection(shapes, pe, npe);
            }
        }

        public void ComputeInterSection(Dictionary<String, Shape> shapes, Vector3 pe, Vector3 npe)
        {
            SortedList<float, String> ts = new SortedList<float, string>(); 
            foreach(String key in shapes.Keys)
            {
                float t = shapes[key].Intersect(pe, npe);
                if (t >= 0f)
                {
                    ts.Add(t, key);
                }
            }
            if (ts.Count == 0)
            {
                Console.WriteLine("No Intersection");
            }
            else 
            {
                float t = ts.Keys[0];
                Console.WriteLine("t: {0} - Shape: {1}", t, ts[t]); 
            }
        }
    }
}
