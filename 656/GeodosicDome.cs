using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.random;
using edu.tamu.courses.imagesynth.core.system;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class GeodosicDome
    {
        private UniformGenerator generator = new UniformGenerator(new Range(-1f, 1f));
        private List<List<Vector3>> directions = new List<List<Vector3>>();
        private Random random = new Random();

        private float samples;

        public GeodosicDome(float m, float n)
        {
            this.samples = m * n;
            this.CreateDome(m, n);
        }

        private void CreateDome(float m, float n)
        {
            DirectoryInfo directory = new DirectoryInfo("../../domes/");
            foreach (FileInfo file in directory.GetFiles("*dome6*"))
            {
                //dome.FileName = "../../domes/dome8.obj";
                Mesh dome = new Mesh();
                dome.FileName = file.FullName;
                dome.LoadFromFile();
                List<Vector3> domePoints = new List<Vector3>();
                foreach (Vector3 point in dome.Points)
                {
                    point.Normalize();
                    domePoints.Add(point);
                }
                directions.Add(domePoints);
            }

            //float theta_chunk = 180f / m;
            //float phi_chunk = 180f / n;

            //float theta = 0f;
            //float phi = 0f;

            //for (int i = 0; i < m; i++)
            //{
            //    theta = i * theta_chunk;
            //    for (int j = 0; j < 2 * n; j++)
            //    {
            //        phi = -180f + j * phi_chunk;
            //        float t = (float)(theta * Math.PI / 180.0);
            //        float p = (float)(phi * Math.PI / 180.0);

            //        Console.WriteLine("({0}, {1})", theta, phi);
            //        double x = Math.Cos(p) * Math.Sin(t);
            //        double y = Math.Sin(p) * Math.Sin(t);
            //        double z = Math.Cos(t);
            //        Vector3 point = new Vector3((float)x, (float)y, (float)z);
            //        if (!directions.Contains(point))
            //        {
            //            directions.Add(point);
            //        }
            //        else
            //        {
            //            Console.WriteLine("Really?");
            //        }
            //    }
            //}
            //
            //Console.WriteLine("\n\n({0}, {1})", theta, phi);
        }

        public List<Vector3> GetDirections(Vector3 n)
        {
            List<Vector3> vectors = new List<Vector3>();
            int index = random.Next(directions.Count);
            foreach (Vector3 direction in directions[index])
            {
                if (direction % n >= 0f)
                {
                    vectors.Add(direction);
                }
            }
            //Console.WriteLine("Normal: {0}", n);
            //for (int i = 0; i < samples; i++)
            //{
            //    Vector3 candidate;
            //    do
            //    {
            //        float x = generator.Next();
            //        float y = generator.Next();
            //        float z = generator.Next();
            //        candidate = new Vector3(x, y, z);
            //    } while (candidate % candidate > 1f || candidate % n < 0f);
            //    candidate.Normalize();
            //    double theta = Math.Acos(candidate.Z);
            //    double phi = Math.Atan2(candidate.Y, candidate.X);
            //    vectors.Add(candidate);
            //    //Console.WriteLine("Candidate: [{0}]", candidate);
            //    //Console.WriteLine("Angle: {0}", Math.Acos(n % candidate) * 180 / Math.PI);
            //}
            return vectors;
        }
    }
}
