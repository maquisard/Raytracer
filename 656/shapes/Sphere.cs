using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Sphere : Shape, UVInterface
    {
        public float Radius { get; set; }
        public Vector3 Center { get; set; }
        public Vector3 Nx { get; set; }
        public Vector3 Ny { get; set; }
        public Vector3 Nz { get; set; }



        public Sphere() : this(1f, Vector3.Zero) {  }

        public Sphere(float radius, Vector3 center)
        {
            this.Radius = radius;
            this.Center = center;
            this.InitLocalBase();
        }

        public Sphere(float radius) : this(radius, Vector3.Zero)
        {
        }

        private void InitLocalBase()
        {
            Nx = new Vector3(1, 0, 0);
            Ny = new Vector3(0, 1, 0);
            Nz = new Vector3(0, 0, 1);
        }

        public float Evaluate(Vector3 p)
        {
            float r = Radius;
            return ((p - Center) % (p - Center)) - r * r;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            if (Math.Abs(npe.Norm - 1f) > 0.0001) throw new ArgumentException(String.Format("The ray vector must be a unit vector. Norm = {0}", npe.Norm));
            Vector3 pc = this.Center;
            float r = Radius;
            float b = npe % (pc - pe);
            float c = (pc - pe) % (pc - pe) - r * r;

            //Console.WriteLine("Value of c: {0}", c);
            //Console.WriteLine("Value of b: {0}", b);

            //if (c < 0) throw new Exception("You are inside the sphere, readjust your camera.");

            float t = -1f; //No Intersection
            if (c > 0.01f)
            {
                float delta = b * b - c;

                // Console.WriteLine("Value of Delta: {0}", delta);

                if (delta >= 0 && b >= 0)
                {
                    t = b - (float)Math.Sqrt(delta);
                }
            }
            return t;
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            Vector3 normal = p - Center;
            normal.Normalize();
            return normal;
        }

        public Vector2 UVCoordinates(Vector3 P)
        {
            float xs = (Nx % (P - Center)) / Radius;
            float ys = (Ny % (P - Center)) / Radius;
            float zs = (Nz % (P - Center)) / Radius;

            float theta = (float)Math.Acos(zs);
            //float theta = (float)Math.Acos(ys / sinPhi);
            float phi = (float)Math.Atan2(Radius * ys, Radius * xs);
            phi = phi < 0 ? (float)(2 * Math.PI + phi) : phi;

            Vector2 uvcoordinates = new Vector2();
            uvcoordinates.X = phi / (float)(2 * Math.PI);
            uvcoordinates.Y = (float)((Math.PI - theta) / Math.PI);

            if (uvcoordinates.X > 1 || uvcoordinates.Y > 1 || float.IsNaN(uvcoordinates.X))
            {
                throw new Exception("This must never happen");
            }

            return uvcoordinates;
        }
    }
}
