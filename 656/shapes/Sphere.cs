using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Sphere : Shape, UVInterface, NormalInterface
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

            float t = -1f; //No Intersection
            if (c > 0.01f)
            {
                float delta = b * b - c;

                if (delta >= 0 && b >= 0)
                {
                    t = b - (float)Math.Sqrt(delta);
                }
            }
            return t;
        }

        public override Vector3 RealNormalAt(Vector3 p)
        {
            Vector3 normal = p - Center;
            normal.Normalize();
            return normal;
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            Vector3 normal = p - Center;
            normal.Normalize();
            if (NormalMap == null)
            {
                return normal;
            }
            else
            {
                return this.GetNormalVector(normal, p, this.UVCoordinates(p));
            }
        }

        public Vector2 UVCoordinates(Vector3 P)
        {
            float xs = (Nx % (P - Center)) / Radius;
            float ys = (Ny % (P - Center)) / Radius;
            float zs = (Nz % (P - Center)) / Radius;

            zs = Math.Abs(Math.Abs(zs) - 1) <= 0.0001 ? (zs > 0 ? 1f : -1f) : zs;
            
            float theta = (float)Math.Acos(zs);
            //float theta = (float)Math.Acos(ys / sinPhi);
            float phi = (float)Math.Atan2(Radius * ys, Radius * xs);
            phi = phi < 0 ? (float)(2 * Math.PI + phi) : phi;

            Vector2 uvcoordinates = new Vector2();
            uvcoordinates.X = phi / (float)(2 * Math.PI);
            uvcoordinates.Y = (float)((Math.PI - theta) / Math.PI);

            if (uvcoordinates.X > 1 || uvcoordinates.Y > 1 || float.IsNaN(uvcoordinates.X) || float.IsNaN(uvcoordinates.Y))
            {
                throw new Exception("This must never happen");
            }

            return uvcoordinates;
        }

        public Vector3 GetNormalVector(Vector3 iNormal, Vector3 iPoint, Vector texCoordinates)
        {
            //return NormalMap.GenerateNormal(iNormal, iPoint);
            Color color = NormalMap.ComputeColor(texCoordinates, iPoint);
            Vector2 uvcoordinates = texCoordinates as Vector2;
            float X = uvcoordinates.X * (float)(NormalMap.Image.Width - 3);
            float Y = uvcoordinates.Y * (float)(NormalMap.Image.Height - 3);

            Vector3 Phx = GetCartesianCoordinates(X + 2, Y);
            Vector3 Phy = GetCartesianCoordinates(X, Y + 2);
            Vector3 Nx = Phx - iPoint; Nx.Normalize();
            Vector3 Ny = Phy - iPoint; Ny.Normalize();

            float nx = 2 * color.R - 1;
            float nz = 2 * color.G - 1;
            float ny = color.B;

            Vector3 NewNormal = iNormal * ny + Nx * nx + Ny * nz;
            NewNormal.Normalize();
            return NewNormal;
        }

        public Vector3 GetCartesianCoordinates(float x, float y)
        {
            float phi = (float)((x / NormalMap.Image.Width) * 2 * Math.PI);
            float theta = (float)((1 - (y / NormalMap.Image.Height)) * Math.PI);

            float r = Radius;
            float xh = Center.X + (float)(r * Math.Cos(phi) * Math.Sin(theta));
            float yh = Center.Y + (float)(r * Math.Sin(phi) * Math.Sin(theta));
            float zh = Center.Z + (float)Math.Cos(theta);
            return new Vector3(xh, yh, zh);
        }

    }
}
